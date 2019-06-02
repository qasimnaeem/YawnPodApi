using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Configuration;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Dto.ResultSet;
using YawnMassage.Common.Domain.Exceptions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services.Configuration
{
    public class PermissionService : IPermissionService
    {
        private readonly IGroupDataContext _dataContext;
        private readonly IPlatformServiceBusService _platformServiceBusService;
        private readonly ILookupService _lookupService;

        public PermissionService(IGroupDataContext dataContext, IPlatformServiceBusService platformServiceBusService, ILookupService lookupService)
        {
            _dataContext = dataContext;
            _platformServiceBusService = platformServiceBusService;
            _lookupService = lookupService;
        }
        
        public async Task<DocumentUpdateResultDto> CreateAsync(PermissionConfigDto dto)
        {
            await CheckForDuplicates(dto.Role);

            var permissionConfig = new PermissionConfig
            {
                Role = dto.Role,
                Permissions = dto.Permissions ?? new List<string>()
            };

            var result = await _dataContext.CreateDocumentAsync(permissionConfig);

            await SendRoleUpdateMessageAsync(dto);

            return result;
        }

        [ExcludeFromCodeCoverage]
        public async Task DeleteAsync(params string[] ids)
        {
            var roleUpdateMessageDto = new RoleUpdateMessageDto {
                Roles = new List<string>()
            };

            var permissionConfigs = await _dataContext.GetDocumentsAsync<PermissionConfig, PermissionConfig>(q=>q.Where(pc=>ids.Contains(pc.Id)));

            foreach (var permissionConfig in permissionConfigs)
            {
                if (permissionConfig != null && !permissionConfig.IsDeleted)
                {
                    permissionConfig.IsDeleted = true;
                    await _dataContext.ReplaceDocumentAsync(permissionConfig);

                    if (permissionConfig.GroupId != "*" && permissionConfig.Permissions.Any(p => p.StartsWith("DEV_CAB")))
                    {
                        roleUpdateMessageDto.GroupId = permissionConfig.GroupId;

                        if (permissionConfig.Role == "*")
                        {
                            roleUpdateMessageDto.Roles.Clear();                            
                            roleUpdateMessageDto.Roles.AddRange(await GetRolesForGroup());
                        }
                        else if (!roleUpdateMessageDto.Roles.Contains(permissionConfig.Role))
                        {
                            roleUpdateMessageDto.Roles.Add(permissionConfig.Role);
                        }                            
                    }
                }
            }

            if (roleUpdateMessageDto.Roles.Any())
                await _platformServiceBusService.TriggerPodAccessDefinitionGenerationAsync(roleUpdateMessageDto);
        }

        public async Task<PermissionConfigDto> GetAsync(string id)
        {
            var document = await _dataContext.GetDocumentAsync<PermissionConfig>(id);

            var dto = new PermissionConfigDto
            {
                Id = document.Id,
                GroupId = document.GroupId,
                Role = document.Role,
                IsDeleted = document.IsDeleted,
                ETag = document.ETag,
                UpdatedById = document.UpdatedById,
                UpdatedByName = document.UpdatedByName,
                UpdatedOnUtc = document.UpdatedOnUtc,
                Permissions = document.Permissions
            };

            return dto;
        }

        public async Task<PagedQueryResultSet<PermissionConfigDto>> GetAsync(ResultSetCriteria gridCriteria, PermissionSearchCriteria searchCriteria)
        {
            var data = await _dataContext.GetDocumentsWithPagingAsync<PermissionConfig, PermissionConfigDto>(q =>
             q.Where(p => (searchCriteria.Role == "any" || p.Role == searchCriteria.Role))
              .Select(p => new PermissionConfigDto
              {
                  GroupId = p.GroupId,
                  Role = p.Role,
                  IsDeleted = p.IsDeleted,
                  Permissions = p.Permissions,
                  Id = p.Id,
                  UpdatedById = p.UpdatedById,
                  UpdatedByName = p.UpdatedByName,
                  UpdatedOnUtc = p.UpdatedOnUtc,
                  ETag = p.ETag
              }), gridCriteria);

            return data;
        }

        public async Task<DocumentUpdateResultDto> UpdateAsync(PermissionConfigDto dto)
        {
            await CheckForDuplicates(dto.Role, dto.Id);

            var permission = await _dataContext.GetDocumentAsync<PermissionConfig>(dto.Id);

            var previousRole = permission.Role;
            var previousDevicePermissions = permission.Permissions.Where(p => p.StartsWith("DEV_CAB"));            

            permission.Id = dto.Id;
            permission.Role = dto.Role;
            permission.Permissions = dto.Permissions ?? new List<string>();
            permission.ETag = dto.ETag;

            var result = await _dataContext.ReplaceDocumentAsync(permission);

            var addedPermissions = new List<string>();
            var deletedPermissions = new List<string>();

            if (dto.GroupId != "*")
            {
                addedPermissions = dto.Permissions.Where(p => p.StartsWith("DEV_CAB") && !previousDevicePermissions.Any(d => d == p)).ToList();
                deletedPermissions = previousDevicePermissions.Where(p => !dto.Permissions.Any(d => d == p)).ToList();

                if ((dto.Role != previousRole && dto.Permissions.Any(p => p.StartsWith("DEV_CAB"))) || addedPermissions.Any() || deletedPermissions.Any())
                {
                    var roleUpdateMessageDto = new RoleUpdateMessageDto
                    {
                        GroupId = dto.GroupId,
                        Roles = (dto.Role == "*" || previousRole == "*") ? await GetRolesForGroup()
                            : (dto.Role == previousRole) ? new List<string> { dto.Role } : new List<string> { dto.Role, previousRole }
                    };

                    await _platformServiceBusService.TriggerPodAccessDefinitionGenerationAsync(roleUpdateMessageDto);
                }
            }

            return result;
        }

        #region Private methods
        
        private async Task CheckForDuplicates(string role, string selfId = null)
        {
            var exists = await _dataContext.AnyAsync<PermissionConfig>(q => q.Where(p =>
                            p.Role == role
                            && (selfId == null || p.Id != selfId)));

            if (exists)
                throw new DocumentDuplicateException();
        }

        private async Task<List<string>> GetRolesForGroup()
        {
            var lookup = await _lookupService.GetLookupListByKeyAsync("LIST_ROLES");
            return lookup.Items.Select(l => l.Value).ToList();
        }

        private async Task SendRoleUpdateMessageAsync(PermissionConfigDto dto)
        {
            if (dto.GroupId != "*" && dto.Permissions.Any(p => p.StartsWith("DEV_CAB")))
            {
                var roleUpdateMessageDto = new RoleUpdateMessageDto
                {
                    GroupId = dto.GroupId,
                    Roles = (dto.Role == "*") ? await GetRolesForGroup() : new List<string> { dto.Role }
                };

                await _platformServiceBusService.TriggerPodAccessDefinitionGenerationAsync(roleUpdateMessageDto);
            }                
        }

        #endregion
    }
}
