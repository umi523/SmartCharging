using SmartCharginModels.Entities;

namespace SmartCharginRepository.Interfaces
{
    public interface IUnitOfWork
    {
        IGroupRepository GroupRepository { get; }
        IChargeStationRepository ChargeStationRepository { get; }
        IConnectorRepository ConnectorRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
