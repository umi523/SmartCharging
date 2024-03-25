using SmartCharginModels.Entities;
using SmartCharginRepository.DatabaseContext;
using SmartCharginRepository.Interfaces;

namespace SmartCharginRepository.Implementations
{
    public class ChargeStationRepository(SmartCharginDbContext context) : Repository<ChargeStation>(context), IChargeStationRepository
    {
    }
}
