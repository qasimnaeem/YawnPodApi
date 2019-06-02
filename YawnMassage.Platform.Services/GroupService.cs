using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Documents.Shared;
using YawnMassage.Platform.Domain.Dto.Group;
using YawnMassage.Platform.Domain.Dto.Shared;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using static YawnMassage.Common.Domain.Constants.SystemKeys;

namespace YawnMassage.Platform.Services
{
    public class GroupService : IGroupService
    {
        private readonly ISystemDataContext _systemDataContext;
        private readonly IConfigurationReaderService _configurationReaderService;
        private readonly RequestContext _requestContext;
        private readonly IUserService _userService;

        public GroupService(ISystemDataContext systemDataContext, IConfigurationReaderService configurationReaderService,
            RequestContext requestContext, IUserService userService)
        {
            _systemDataContext = systemDataContext;
            _configurationReaderService = configurationReaderService;
            _requestContext = requestContext;
            _userService = userService;
        }

        public async Task<PagedQueryResultSet<GroupDto>> GetAsync(ResultSetCriteria gridCriteria, GroupSearchCriteria searchCriteria)
        {
            string searchText = searchCriteria.Name?.Trim().ToUpper() ?? string.Empty;

            var groups = await _systemDataContext.GetDocumentsWithPagingAsync<Group, Group>(q =>
                    q.Where(d => searchText == string.Empty || d.Name.ToUpper().Contains(searchText)),
                    gridCriteria);

            var groupDtoList = groups.Results.Select(c => new GroupDto
            {
                Id = c.Id,
                Name = c.Name,
                FirstName = c.FirstName,
                LastName = c.LastName,
                FullName = c.FullName,
                MobileNumber = new MobileNumberDto
                {
                    IddCode = c.MobileNumber?.IddCode,
                    Number = c.MobileNumber?.Number
                },
                Email = c.Email,
                IsDeleted = c.IsDeleted,
                UpdatedById = c.UpdatedById,
                UpdatedByName = c.UpdatedByName,
                UpdatedOnUtc = c.UpdatedOnUtc,
                ETag = c.ETag
            }).ToList();

            return new PagedQueryResultSet<GroupDto> { Results = groupDtoList, ContinuationToken = groups.ContinuationToken };
        }

        public async Task<GroupDto> GetAsync(string id)
        {
            var group = await _systemDataContext.GetDocumentAsync<Group>(id);

            if (group == null)
                return null;

            var dto = new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                FirstName = group.FirstName,
                LastName = group.LastName,
                MobileNumber = new MobileNumberDto
                {
                    IddCode = group.MobileNumber?.IddCode,
                    Number = group.MobileNumber?.Number
                },
                Email = group.Email,
                IsDeleted = group.IsDeleted,
                UpdatedById = group.UpdatedById,
                UpdatedByName = group.UpdatedByName,
                UpdatedOnUtc = group.UpdatedOnUtc,
                ETag = group.ETag
            };

            return dto;
        }

        public async Task<DocumentUpdateResultDto> CreateAsync(GroupDto groupDto)
        {
            ValidateGroup(groupDto);

            var tag = groupDto.Tag?.Trim();

            if (string.IsNullOrEmpty(tag))
                tag = await GetUniqueTagAsync(groupDto.Name);
            else
                await ValidateTagUniquenessAsync(groupDto.Tag);

            var group = new Group
            {
                Name = groupDto.Name,
                FirstName = groupDto.FirstName,
                LastName = groupDto.LastName,
                FullName = string.Format("{0} {1}", groupDto.FirstName, groupDto.LastName),
                MobileNumber = new MobileNumber
                {
                    IddCode = groupDto.MobileNumber?.IddCode,
                    Number = groupDto.MobileNumber?.Number
                },
                Email = groupDto.Email,
                Tag = tag
            };

            var result = await _systemDataContext.CreateDocumentAsync(group);

            await CreateDefaultGroupRoleForLoggedInUser(result.Id);

            return result;
        }

        public async Task<DocumentUpdateResultDto> UpdateAsync(GroupDto groupDto)
        {
            ValidateGroup(groupDto);

            var group = await _systemDataContext.GetDocumentAsync<Group>(groupDto.Id);
            group.Name = groupDto.Name;
            group.FirstName = groupDto.FirstName;
            group.LastName = groupDto.LastName;
            group.FullName = string.Format("{0} {1}", groupDto.FirstName, groupDto.LastName);
            group.MobileNumber = new MobileNumber
            {
                IddCode = groupDto.MobileNumber?.IddCode,
                Number = groupDto.MobileNumber?.Number
            };
            group.Email = groupDto.Email;
            group.ETag = groupDto.ETag;

            var result = await _systemDataContext.ReplaceDocumentAsync(group);
            return result;
        }

        [ExcludeFromCodeCoverage]
        public async Task DeleteAsync(params string[] ids)
        {
            await _systemDataContext.SoftDeleteDocumentsAsync<Group>(ids);
        }

        #region Support Methods

        private async Task ValidateTagUniquenessAsync(string tag)
        {
            if (await _systemDataContext.AnyAsync<Group>(q => q.Where(c => c.Tag == tag)))
                throw new YawnMassageException("ERROR_DUPLICATE_TAG");
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
                var tagExists = await _systemDataContext.AnyAsync<Group>(q => q.Where(c => c.Tag == tag), true);
                if (!tagExists)
                    break;

                tag = tagCandidate + counter++;
            }

            return tag;
        }

        private void ValidateGroup(GroupDto group)
        {
            if (string.IsNullOrEmpty(group.Name))
            {
                throw new YawnMassageException("ERROR_GROUP_NAME_REQUIRED");
            }

            if (string.IsNullOrEmpty(group.FirstName))
            {
                throw new YawnMassageException("ERROR_FIRST_NAME_REQUIRED");
            }

            if (string.IsNullOrEmpty(group.LastName))
            {
                throw new YawnMassageException("ERROR_LAST_NAME_REQUIRED");
            }

            if (string.IsNullOrEmpty(group.Email))
            {
                throw new YawnMassageException("ERROR_EMAIL_REQUIRED");
            }

            if (string.IsNullOrEmpty(group.MobileNumber?.IddCode) || string.IsNullOrEmpty(group.MobileNumber?.Number))
            {
                throw new YawnMassageException("ERROR_MOBILE_NUMBER_REQUIRED");
            }
        }

        private async Task CreateDefaultGroupRoleForLoggedInUser(string groupId)
        {
            var defaultGroupRole = await _configurationReaderService.GetValueAsync(ConfigurationKeys.DefaultGroupRole);
            await _userService.AddGroupRoleToUserAsync(_requestContext.UserId,
                                        new UserGroupRole { Group = groupId, Role = defaultGroupRole });
        }

        #endregion
    }
}
