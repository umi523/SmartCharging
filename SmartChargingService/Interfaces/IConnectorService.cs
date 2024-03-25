using SmartCharginModels.Models;

namespace SmartChargingService.Interfaces
{
    public interface IConnectorService
    {
        Task<bool> RemoveAsync(int connectorId);
        Task<IEnumerable<ConnectorViewModel>> GetAllAsync();
        Task<ConnectorViewModel> GetByIdAsync(int connectorId);
        Task<ConnectorViewModel> AddAsync(int connectorId, int newMaxCurrentInAmps);
        Task<bool> UpdateAsync(int connectorId, int newMaxCurrentInAmps);
    }
}