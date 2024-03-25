using SmartCharginModels.Entities;
using SmartCharginModels.Models;

namespace SmartChargingService.Interfaces
{
    public interface IGroupService
    {
        Task<GroupViewModel> AddAsync(GroupPostModel group);
        Task<bool> RemoveAsync(int groupId);
        Task<GroupViewModel> GetByIdAsync(int groupId);
        Task<IEnumerable<GroupViewModel>> GetAllAsync();
        Task<GroupViewModel> UpdateAsync(int groupId, Group group);
        Task<int> GetSumOfConnectorsMaxCurrentInAmpsByGroupId(int groupId);
    }
}