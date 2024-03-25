using SmartCharginModels.Models;

namespace SmartChargingService.Interfaces
{
    public interface IChargeStationService
    {
        Task<bool> RemoveAsync(int chargeStationId);
        Task<IEnumerable<ChargeStationViewModel>> GetAllAsync();
        Task<ChargeStationViewModel> GetByIdAsync(int chargeStationId);
        Task<ChargeStationViewModel> AddAsync(int groupId, ChargeStationPostModel chargeStationViewModel);
        Task<ChargeStationViewModel> UpdateAsync(int chargeStationId, ChargeStationViewModel chargeStationViewModel);
    }
}