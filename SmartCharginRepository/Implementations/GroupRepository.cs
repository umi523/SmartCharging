using SmartCharginModels.Entities;
using SmartCharginRepository.DatabaseContext;
using SmartCharginRepository.Interfaces;

namespace SmartCharginRepository.Implementations
{
    public class GroupRepository(SmartCharginDbContext context) : Repository<Group>(context), IGroupRepository
    {
    }
}
