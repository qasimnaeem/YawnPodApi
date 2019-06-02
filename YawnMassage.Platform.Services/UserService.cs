using YawnMassage.Platform.Domain.Constants;
using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Documents.Shared;
using YawnMassage.Platform.Domain.Dto.Shared;
using YawnMassage.Platform.Domain.Dto.User;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Exceptions;
using YawnMassage.Common.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Documents;
using YawnMassage.Platform.Domain.Dto.Identity;
using static YawnMassage.Common.Domain.Constants.SystemKeys;
using User = YawnMassage.Platform.Domain.Documents.User;

namespace YawnMassage.Platform.Services
{
    public class UserService : IUserService
    {
        private readonly ISystemDataContext _systemDataContext;
        private readonly IIdentityService _identityService;
        private readonly IConfigurationReaderService _configurationReaderService;
        private readonly RequestContext _requestContext;
        private readonly IUserContextService _webUserContextService;
        private readonly IPlatformServiceBusService _platformServiceBusService;

        private const string Permission_Search = "USER_SEARCH";
        private const string Permission_New = "USER_NEW";
        private const string Permission_Update = "USER_UPDATE";


        public UserService(ISystemDataContext systemDataContext, IIdentityService identityService,
            IConfigurationReaderService configurationReaderService, RequestContext requestContext,
            IUserContextService webUserContextService, IPlatformServiceBusService platformServiceBusService)
        {
            _systemDataContext = systemDataContext;
            _configurationReaderService = configurationReaderService;
            _identityService = identityService;
            _requestContext = requestContext;
            _webUserContextService = webUserContextService;
            _platformServiceBusService = platformServiceBusService;
        }

        [ExcludeFromCodeCoverage] //We exclude this from CodeCoverage because LINQ ARRAY_CONTAINS is not supported in unit tests.
        public async Task<PagedQueryResultSet<UserDto>> GetAsync(ResultSetCriteria gridCriteria, UserSearchCriteria searchCriteria)
        {
            string searchText = searchCriteria.Name?.Trim().ToUpper() ?? string.Empty;

            #region GroupRoles ARRAY_CONTAINS expression support

            var groupId = _requestContext.GroupId;

            JObject customRoleMatch = new JObject();
            Dictionary<string, object> arrayContainsMatches = null;
            if (!string.IsNullOrEmpty(groupId))
                customRoleMatch["group"] = groupId;
            if (!string.IsNullOrEmpty(searchCriteria.Role) && searchCriteria.Role != "any")
                customRoleMatch["role"] = searchCriteria.Role;

            var skipGroupRoleMatch = (customRoleMatch.Properties().Count() == 0);
            if (!skipGroupRoleMatch)
                arrayContainsMatches = new Dictionary<string, object> { { "groupRoles", customRoleMatch } };

            #endregion

            var users = await _systemDataContext.GetDocumentsWithPagingAsync<User, User>(q =>
                q.Where(u => (searchText == string.Empty || u.NormalizedFullName.Contains(searchText))
                        && (skipGroupRoleMatch || u.GroupRoles.Contains(null)))
                    , gridCriteria, arrayContainsMatches);

            var allowedGroups = await GetAllowedGroupIdsForLoggedInUserAsync(Permission_Search);

            var userDtoList = users.Results.Select(u => new UserDto
            {
                Id = u.Id,
                IsDeleted = u.IsDeleted,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FullName = u.FullName,
                UpdatedById = u.UpdatedById,
                UpdatedByName = u.UpdatedByName,
                UpdatedOnUtc = u.UpdatedOnUtc,
                GroupRoles = u.GroupRoles
                    .Where(cr => allowedGroups.Contains(cr.Group)) //Only show roles from allowed groups
                    .Select(cr => new UserGroupRoleDto
                    {
                        GroupId = cr.Group,
                        Role = cr.Role
                    }).OrderBy(cr => cr.GroupId).ThenBy(cr => cr.Role).ToList()
            }).ToList();

            return new PagedQueryResultSet<UserDto> { Results = userDtoList, ContinuationToken = users.ContinuationToken };
        }

        public async Task<UserDto> GetUserForEditAsync(string id)
        {
            var user = await _systemDataContext.GetDocumentAsync<User>(id);

            //Validate access
            await CheckAccessToUserAsync(user, Permission_Update);

            var allowedGroups = await GetAllowedGroupIdsForLoggedInUserAsync(Permission_Update);

            var dto = new UserDto
            {
                Id = user.Id,
                IsDeleted = user.IsDeleted,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Culture = user.Culture,
                TimeZone = user.TimeZone,
                MobileNumber = new MobileNumberDto
                {
                    IddCode = user.MobileNumber?.IddCode,
                    Number = user.MobileNumber?.Number
                },
                AlternateId = user.AlternateId,
                Email = user.Email,
                IsEmailConfirmed = user.IsEmailConfirmed,
                IsActivationEmailSent = user.IsActivationEmailSent,
                AccessExpiryDate = user.AccessExpiryDate,
                UpdatedById = user.UpdatedById,
                UpdatedByName = user.UpdatedByName,
                UpdatedOnUtc = user.UpdatedOnUtc,
                ETag = user.ETag,
                GroupRoles = user.GroupRoles?
                    .Where(cr => allowedGroups.Contains(cr.Group)) //Only show roles from allowed groups
                    .Select(cr => new UserGroupRoleDto
                    {
                        GroupId = cr.Group,
                        Role = cr.Role
                    }).ToList()
            };

            return dto;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            var user = await _systemDataContext.GetDocumentAsync<User>(userId);
            return user;
        }

        public async Task<DocumentUpdateResultDto> CreateAsync(UserDto userDto, bool isBulkImport = false)
        {
            var result = new DocumentUpdateResultDto();
            try
            {
                ValidateUser(userDto);
                await CheckForDuplicatesAsync(userDto.Email, userDto.AlternateId);
                await ValidatePINAsync(userDto.PIN, userDto.GroupRoles.Select(cr => cr.GroupId).ToList());

                var roleAssignments = await CreateUserGroupRolesFromDtoAsync(userDto.GroupRoles,
                    isBulkImport ? BulkData.PermissionKey : Permission_New);
                ValidateUserRoles(roleAssignments);

                var tag = userDto.Tag?.Trim();

                if (string.IsNullOrEmpty(tag))
                    tag = await GetUniqueTagAsync(userDto.FirstName);
                else
                    await ValidateTagUniquenessAsync(userDto.Tag);

                var user = new User
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    FullName = string.Format("{0} {1}", userDto.FirstName, userDto.LastName),
                    NormalizedFullName = string.Format("{0} {1}", userDto.FirstName, userDto.LastName).ToUpper(),
                    Culture = userDto.Culture,
                    PIN = !string.IsNullOrWhiteSpace(userDto.PIN) ? CryptographicProvider.GenerateUserPINHash(userDto.PIN) : null,
                    TimeZone = userDto.TimeZone,
                    MobileNumber = new MobileNumber
                    {
                        IddCode = userDto.MobileNumber?.IddCode,
                        Number = userDto.MobileNumber?.Number
                    },
                    AlternateId = userDto.AlternateId,
                    Email = userDto.Email,
                    UserName = userDto.Email,
                    NormalizedUserName = userDto.Email.ToUpper(),
                    NormalizedEmail = userDto.Email.ToUpper(),
                    AccessExpiryDate = userDto.AccessExpiryDate,
                    GroupRoles = roleAssignments,
                    UserLocation = new UserLocation
                    {
                        Country = userDto.UserLocation?.Country,
                        State = userDto.UserLocation?.State,
                        City = userDto.UserLocation?.City
                    },
                    Tag = tag,
                    ImageBlobId = userDto.ImageBlobId,
                    PasswordHash = userDto.PasswordHash
                };
                if (userDto.Purposes != null && userDto.Purposes.Count > 0)
                {
                    user.Purposes = new List<string>();
                    user.Purposes.AddRange(userDto.Purposes);
                }

                await _identityService.CreateUserAsync(user);

                var userUpdateMessageDto = new UserUpdateMessageDto
                {
                    UserId = user.Id,
                    GroupRoles = userDto.GroupRoles
                };

                //await _platformServiceBusService.TriggerPodAccessDefinitionGenerationAsync(userUpdateMessageDto);
                result.IsSucceeded = true;
                result.Id = user.Id;
                result.ETag = user.ETag;
                result.UpdatedById = user.UpdatedById;
                result.UpdatedOnUtc = user.UpdatedOnUtc;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.IsSucceeded = false;
                result.ErrorCode = e.Message;
            }

            return result;
        }

        public async Task<DocumentUpdateResultDto> UpdateAsync(UserDto userDto, bool isBulkImport = false)
        {
            ValidateUser(userDto);
            await CheckForDuplicatesAsync(userDto.Email, userDto.AlternateId, userDto.Id);

            var user = await _systemDataContext.GetDocumentAsync<User>(userDto.Id);
            //Validate access
            await CheckAccessToUserAsync(user, isBulkImport ? BulkData.PermissionKey : Permission_Update);
            var previousRoles = user.GroupRoles;
            var previousPIN = user.PIN;
            var previousFirstName = user.FirstName;
            var previousLastName = user.LastName;
            var previousAlternateId = user.AlternateId;

            await ValidatePINAsync(userDto.PIN, user.GroupRoles.Select(cr => cr.Group).ToList());

            var roleAssignments = await GetGroupRolesToUpdateUserAsync(user.GroupRoles, userDto.GroupRoles, isBulkImport);
            ValidateUserRoles(roleAssignments);

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.FullName = string.Format("{0} {1}", userDto.FirstName, userDto.LastName);
            user.Culture = userDto.Culture;
            if (!string.IsNullOrEmpty(userDto.PIN))
                user.PIN = CryptographicProvider.GenerateUserPINHash(userDto.PIN);

            user.TimeZone = userDto.TimeZone;
            user.MobileNumber = new MobileNumber
            {
                IddCode = userDto.MobileNumber?.IddCode,
                Number = userDto.MobileNumber?.Number
            };
            user.AlternateId = userDto.AlternateId;
            user.Email = userDto.Email;
            user.AccessExpiryDate = userDto.AccessExpiryDate;
            user.IsDeleted = userDto.IsDeleted;
            user.GroupRoles = roleAssignments;
            user.ETag = userDto.ETag;
            user.ImageBlobId = userDto.ImageBlobId;
            if (userDto.Purposes != null && userDto.Purposes.Count > 0)
            {
                user.Purposes=new List<string>();
                user.Purposes.AddRange(userDto.Purposes);
            }
            
            var result = await _systemDataContext.ReplaceDocumentAsync(user);
            var addedRoles = user.GroupRoles.Where(cr => !previousRoles.Any(r => r.Group == cr.Group && r.Role == cr.Role));
            var deletedRoles = previousRoles.Where(cr => !user.GroupRoles.Any(r => r.Group == cr.Group && r.Role == cr.Role));

            bool normalizationRequired =
                userDto.PIN != previousPIN ||
                userDto.FirstName != previousFirstName ||
                userDto.LastName != previousLastName ||
                userDto.AlternateId != previousAlternateId ||
                addedRoles.Any() ||
                deletedRoles.Any();

            if (normalizationRequired)
            {
                var userUpdateMessageDto = new UserUpdateMessageDto
                {
                    UserId = user.Id,
                    GroupRoles = new List<UserGroupRoleDto>()
                };

                if (userDto.PIN != previousPIN || userDto.FirstName != previousFirstName || userDto.LastName != previousLastName || userDto.AlternateId != previousAlternateId)
                    userUpdateMessageDto.GroupRoles.AddRange(user.GroupRoles.Select(cr => new UserGroupRoleDto { GroupId = cr.Group, Role = cr.Role }));

                else if (addedRoles.Any())
                    userUpdateMessageDto.GroupRoles.AddRange(addedRoles.Select(cr => new UserGroupRoleDto { GroupId = cr.Group, Role = cr.Role }));

                if (deletedRoles.Any())
                    userUpdateMessageDto.GroupRoles.AddRange(deletedRoles.Select(cr => new UserGroupRoleDto { GroupId = cr.Group, Role = cr.Role }));

                await _platformServiceBusService.TriggerPodAccessDefinitionGenerationAsync(userUpdateMessageDto);
            }

            return result;
        }

        [ExcludeFromCodeCoverage]
        public async Task DeleteAsync(params string[] ids)
        {
            foreach (string id in ids)
            {
                var user = await _systemDataContext.GetDocumentAsync<User>(id);

                if (user != null && !user.IsDeleted)
                {
                    user.IsDeleted = true;
                    await _systemDataContext.ReplaceDocumentAsync(user);

                    var userUpdateMessageDto = new UserUpdateMessageDto
                    {
                        UserId = user.Id,
                        GroupRoles = user.GroupRoles.Select(cr => new UserGroupRoleDto { GroupId = cr.Group, Role = cr.Role }).ToList()
                    };

                    await _platformServiceBusService.TriggerPodAccessDefinitionGenerationAsync(userUpdateMessageDto);
                }
            }
        }

        public async Task<DocumentUpdateResultDto> ActivateUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new YawnMassageException("ERROR_USERID_CANNOTBE_NULL");

            var user = await _systemDataContext.GetDocumentAsync<User>(userId);

            //Validate access
            await CheckAccessToUserAsync(user, Permission_Update);

            if (user == null)
                throw new YawnMassageException("ERROR_NO_USER_FOUND");

            if (user.IsEmailConfirmed)
                throw new YawnMassageException("ERROR_USER_ALREADY_ACTIVE");

            await _identityService.SendUserActivationEmailAsync(user);

            return new DocumentUpdateResultDto
            {
                Id = user.Id,
                ETag = user.ETag,
                UpdatedByName = user.UpdatedByName,
                UpdatedOnUtc = user.UpdatedOnUtc
            };
        }

        public async Task<int> GetRandomPINAsync(string group)
        {
            Random random = new Random();

            var pinLengthFromConfig = await GetPINLengthForGivenGroup(group);

            int.TryParse(GenerateVariableLength(pinLengthFromConfig, true), out int minBoundry);

            int.TryParse(GenerateVariableLength(pinLengthFromConfig, false), out int maxBoundry);

            if (minBoundry == 0 || maxBoundry == 0)
            {
                throw new YawnMassageException("ERROR_INVALID_PIN_CONFIGURATION");
            }

            int rInt = random.Next(minBoundry, maxBoundry);

            return rInt;
        }

        public async Task<string> GetEffectiveGroupForUserAsync(string userId)
        {
            var user = await _systemDataContext.GetDocumentAsync<User>(userId);
            var distinctGroupIds = user.GroupRoles.Select(cr => cr.Group).Distinct().ToList();

            if (distinctGroupIds.Count == 1)
            {
                return distinctGroupIds.First();
            }
            else
            {
                return "*";
            }
        }

        public async Task AddGroupRoleToUserAsync(string userId, UserGroupRole groupRole)
        {
            var user = await _systemDataContext.GetDocumentAsync<User>(userId);

            if (user.GroupRoles.Any(cr => cr.Group == groupRole.Group && cr.Role == groupRole.Role))
                throw new YawnMassageException("ERROR_GROUPROLE_DUPLICATE_VALUE");

            user.GroupRoles.Add(groupRole);
            await _systemDataContext.ReplaceDocumentAsync(user);

            await _identityService.RefreshSigninForCurrentUserAsync();
        }

        public async Task<bool> CheckIfAccountExistAsync(string mobileNumber)
        {
            var exists = await _systemDataContext.AnyAsync<User>(u => u.Where(l => !l.IsDeleted && l.MobileNumber.Number == mobileNumber));
            return exists;
        }

        public async Task<MobileAuthResultDto> GetPostAuthDataAsync(string mobileNumber)
        {
            var result=new MobileAuthResultDto();
            try
            {
                var user = await _systemDataContext.FirstOrDefaultAsync<User, User>(doc
                     => doc.Where(l => !l.IsDeleted && l.MobileNumber.Number == mobileNumber
                 ));
                result.UserDto = new UserDto
                {
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Culture = user.Culture,
                    TimeZone = user.TimeZone,
                    MobileNumber = new MobileNumberDto
                    {
                        IddCode = user.MobileNumber?.IddCode,
                        Number = user.MobileNumber?.Number
                    },
                    UserLocation=new UserLocationDto
                    {
                        Country=user.UserLocation?.Country,
                        State=user.UserLocation?.State,
                        City=user.UserLocation?.City
                    },
                    Purposes=new List<string>(user.Purposes!=null?user.Purposes:new List<string>()),
                    AlternateId = user.AlternateId,
                    Email = user.Email,
                    IsEmailConfirmed = user.IsEmailConfirmed,
                    IsActivationEmailSent = user.IsActivationEmailSent,
                    AccessExpiryDate = user.AccessExpiryDate,
                    UpdatedById = user.UpdatedById,
                    UpdatedByName = user.UpdatedByName,
                    UpdatedOnUtc = user.UpdatedOnUtc,
                    ImageBlobId = user.ImageBlobId,
                    ETag = user.ETag,
                    GroupRoles = user.GroupRoles//Only show roles from allowed groups
                        .Select(cr => new UserGroupRoleDto
                        {
                            GroupId = cr.Group,
                            Role = cr.Role
                        }).ToList()
                };
                if (user.Purposes != null && user.Purposes.Count > 0)
                {
                    result.UserDto.Purposes = new List<string>();
                    result.UserDto.Purposes.AddRange(user.Purposes);
                }
                result.Token = await _identityService.GetUserTokenAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        public async Task<List<ListItemDto>> GetUsersByGroupAsync(string groupId)
        {
            var allowedGroups = await GetAllowedGroupIdsForLoggedInUserAsync(Permission_Search);

            var canAccessGroup = allowedGroups.Any(cr => cr == groupId);

            if (!canAccessGroup)
                throw new UnauthorizedAccessException();

            #region GroupRoles ARRAY_CONTAINS expression support

            JObject customRoleMatch = new JObject();
            Dictionary<string, object> arrayContainsMatches = null;
            if (!string.IsNullOrEmpty(groupId))
                customRoleMatch["group"] = groupId;

            arrayContainsMatches = new Dictionary<string, object> { { "groupRoles", customRoleMatch } };

            #endregion

            var userList = await _systemDataContext.GetDocumentsAsync<User, ListItemDto>(q =>
            q.Where(u => u.GroupRoles.Contains(null)).Select(u => new ListItemDto
            {
                Id = u.Id,
                Name = u.FullName,
            }), false, arrayContainsMatches);

            return userList;
        }
               
        [ExcludeFromCodeCoverage]
        public async Task<List<string>> GetUserIdsByGroupRoleAsync(string groupId, string role)
        {
            #region GroupRoles ARRAY_CONTAINS expression support

            JObject customRoleMatch = new JObject();
            Dictionary<string, object> arrayContainsMatches = null;
            customRoleMatch["group"] = groupId;
            customRoleMatch["role"] = role;

            arrayContainsMatches = new Dictionary<string, object> { { "groupRoles", customRoleMatch } };

            #endregion

            var userList = await _systemDataContext.GetDocumentsAsync<User, string>(q =>
                q.Where(u => u.GroupRoles.Contains(null)).Select(u => u.Id), false, arrayContainsMatches);

            return userList;
        }

        #region Support Methods

        private async Task ValidateTagUniquenessAsync(string tag)
        {
            if (await _systemDataContext.AnyAsync<User>(q => q.Where(c => c.Tag == tag)))
                throw new YawnMassageException("DUPLICATE_TAG");
        }
        private async Task<string> GetUniqueTagAsync(string tagCandidate)
        {
            tagCandidate = tagCandidate.ToLower().Trim();
            tagCandidate = tagCandidate.Split(' ').First(); //Get first word.

            if (tagCandidate.Length > 6)
                tagCandidate = tagCandidate.Substring(0, 6);

            var tag = tagCandidate;
            var counter = 2;

            while (true)
            {
                var tagExists = await _systemDataContext.AnyAsync<User>(q => q.Where(c => c.Tag == tag), true);
                if (!tagExists)
                    break;

                tag = tagCandidate + counter++;
            }

            return tag;
        }

        private void ValidateUser(UserDto user)
        {
            if (string.IsNullOrEmpty(user.Culture))
            {
                throw new YawnMassageException("ERROR_USER_CULTURE_REQUIRED");
            }

            if (string.IsNullOrEmpty(user.TimeZone))
            {
                throw new YawnMassageException("ERROR_USER_TIMEZONE_REQUIRED");
            }

            if (string.IsNullOrEmpty(user.FirstName))
            {
                throw new YawnMassageException("ERROR_USER_FIRST_NAME_REQUIRED");
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                throw new YawnMassageException("ERROR_USER_LAST_NAME_REQUIRED");
            }

            if (string.IsNullOrEmpty(user.MobileNumber?.IddCode) || string.IsNullOrEmpty(user.MobileNumber?.Number))
            {
                throw new YawnMassageException("ERROR_USER_MOBILE_NUMBER_REQUIRED");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new YawnMassageException("ERROR_USER_EMAIL_REQUIRED");
            }

            if (string.IsNullOrEmpty(user.AlternateId))
            {
                throw new YawnMassageException("ERROR_USER_ALTERNATE_ID_REQUIRED");
            }
        }



        private async Task<IEnumerable<string>> GetAllowedGroupIdsForLoggedInUserAsync(string permissionKey)
        {
            var claims = await _webUserContextService.GetClaimsForCurrentUserAsync();

            var allowedGroupIds = claims
                .Where(c => c.EndsWith(permissionKey))
                .Select(c => c.Split(':')[0]).Distinct();
            return allowedGroupIds;
        }

        private void ValidateUserRoles(List<UserGroupRole> groupRoles)
        {
            if (groupRoles == null || groupRoles.Count == 0)
                throw new YawnMassageException("ERROR_USER_NO_ROLES_SPECIFIED");
        }

        private async Task ValidatePINAsync(string pin, List<string> groupIds)
        {
            if (!string.IsNullOrEmpty(pin))
            {
                var requiredPINLength = await GetPINLengthForGivenGroup(
                        groupIds.Count == 1 ? groupIds[0] : null);

                if (requiredPINLength != pin.Trim().Length)
                {
                    throw new YawnMassageException("ERROR_INVALID_PIN");
                }
            }
        }

        private async Task<int> GetPINLengthForGivenGroup(string group)
        {
            int.TryParse(await _configurationReaderService.GetValueAsync(ConfigurationValueType.pinLength, group),
                            out int pinLengthFromConfig);

            if (pinLengthFromConfig == 0)
            {
                pinLengthFromConfig = 4;
            }
            return pinLengthFromConfig;
        }

        private string GenerateVariableLength(int requiredLength, Boolean isMin)
        {
            string boundryValue = string.Empty;

            for (int i = 0; i < requiredLength; i++)
            {
                boundryValue += isMin ? "1" : "9";
            }

            return boundryValue;
        }

        private async Task CheckAccessToUserAsync(User user, string permissionKey)
        {
            var allowedGroupIds = await GetAllowedGroupIdsForLoggedInUserAsync(permissionKey);

            //Check whether the given user is any any role from allowed group Ids.
            var accessAllowed = user.GroupRoles.Any(cr => allowedGroupIds.Contains(cr.Group));
            if (!accessAllowed)
                throw new UnauthorizedAccessException();
        }

        private async Task<List<UserGroupRole>> GetGroupRolesToUpdateUserAsync(IEnumerable<UserGroupRole> original, IEnumerable<UserGroupRoleDto> toSaveDto, bool isBulkImport)
        {
            //The list containing all the group roles that needs to be added
            //among allowed groups for the logged in user.
            var allowedGroupRoles = await CreateUserGroupRolesFromDtoAsync(toSaveDto, isBulkImport ? BulkData.PermissionKey : Permission_Update);

            //Get the subset of other group roles (to which current user is not allowed) from the original set.
            var allowedGroupIds = await GetAllowedGroupIdsForLoggedInUserAsync(isBulkImport ? BulkData.PermissionKey : Permission_Update);
            var otherGroupRoles = original.Where(cr => !allowedGroupIds.Contains(cr.Group));

            if (isBulkImport)
            {
                //find the roles those were not passed from the excel sheet due to the pre-selection of Group
                var rolesNotExposed = from org in original
                                      from ts in toSaveDto
                                      where org.Group != ts.GroupId
                                      select org;

                allowedGroupRoles = allowedGroupRoles.Union(rolesNotExposed).ToList();
            }

            //We need to overwrite the database user with the merge of both group role set.
            return allowedGroupRoles.Union(otherGroupRoles).ToList();

        }

        private async Task<List<UserGroupRole>> CreateUserGroupRolesFromDtoAsync(IEnumerable<UserGroupRoleDto> userGroupRoles, string permission)
        {
            var allowedGroups = await GetAllowedGroupIdsForLoggedInUserAsync(permission);
            var containsUnauthorizedGroups = userGroupRoles != null && allowedGroups!=null && allowedGroups.ToList().Count>0 && userGroupRoles.Any(cr => !allowedGroups.Contains(cr.GroupId));

            if (containsUnauthorizedGroups)
                throw new UnauthorizedAccessException();

            var data = userGroupRoles?.Select(cr => new UserGroupRole
            {
                Group = cr.GroupId,
                Role = cr.Role
            }).ToList();

            return data ?? new List<UserGroupRole>();
        }

        private async Task CheckForDuplicatesAsync(string email, string alternateId, string selfId = null)
        {
            var exists = await _systemDataContext.AnyAsync<User>(u => u.Where(l => !l.IsDeleted
                            && (l.Email == email || l.AlternateId == alternateId)
                            && (selfId == null || l.Id != selfId)));

            if (exists)
                throw new DocumentDuplicateException();
        }

        #endregion
    }
}
