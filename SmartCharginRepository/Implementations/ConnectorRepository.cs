using SmartCharginModels.Entities;
using SmartCharginRepository.DatabaseContext;
using SmartCharginRepository.Interfaces;

namespace SmartCharginRepository.Implementations
{
    public class ConnectorRepository(SmartCharginDbContext context) : Repository<Connector>(context), IConnectorRepository
    {
    }
}
