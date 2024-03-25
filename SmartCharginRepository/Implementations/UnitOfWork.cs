using SmartCharginRepository.DatabaseContext;
using SmartCharginRepository.Interfaces;

namespace SmartCharginRepository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartCharginDbContext _context;

        public UnitOfWork(SmartCharginDbContext context)
        {
            _context = context;
            GroupRepository = new GroupRepository(_context);
            ConnectorRepository = new ConnectorRepository(_context);
            ChargeStationRepository = new ChargeStationRepository(_context);
        }

        public IGroupRepository GroupRepository { get; }
        public IConnectorRepository ConnectorRepository { get; }
        public IChargeStationRepository ChargeStationRepository { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
