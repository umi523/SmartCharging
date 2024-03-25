using AutoMapper;
using SmartChargingService.Interfaces;
using SmartCharginModels.Entities;
using SmartCharginModels.Models;
using SmartCharginRepository.Interfaces;

namespace SmartChargingService.Implementations
{
    public class ConnectorService(IMapper mapper, IUnitOfWork unitOfWork,
        IGroupService groupService, IChargeStationService chargeStationService) : IConnectorService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGroupService _groupService = groupService;
        private readonly IChargeStationService _chargeStationService = chargeStationService;

        public async Task<ConnectorViewModel> AddAsync(int chargeStationId, int newMaxCurrentInAmps)
        {
            var chargeStation = await _chargeStationService.GetByIdAsync(chargeStationId);
            if (chargeStation.Connectors?.Count > 5)
            {
                throw new ArgumentException("Charging Station can have only 5 connectors");
            }

            var connector = new Connector() { ChargeStationId = chargeStation.Id, MaxCurrentInAmps = newMaxCurrentInAmps };
            return _mapper.Map<ConnectorViewModel>(await _unitOfWork.ConnectorRepository.AddAsync(connector));
        }

        public async Task<bool> RemoveAsync(int connectorId)
        {
            var connectorToRemove = await _unitOfWork.ConnectorRepository.GetByIdAsync(connectorId) ?? throw new ArgumentException("Connector not found");
            return await _unitOfWork.ConnectorRepository.RemoveAsync(connectorToRemove);
        }

        public async Task<ConnectorViewModel> GetByIdAsync(int connectorId)
        {
            return _mapper.Map<ConnectorViewModel>(await _unitOfWork.ConnectorRepository.GetByIdAsync(connectorId));
        }

        public async Task<IEnumerable<ConnectorViewModel>> GetAllAsync()
        {
            return _mapper.Map<List<ConnectorViewModel>>(await _unitOfWork.ConnectorRepository.GetAllAsync());
        }

        public async Task<bool> UpdateAsync(int connectorId, int newMaxCurrentInAmps)
        {
            var connector = await _unitOfWork.ConnectorRepository.GetByIdAsync(connectorId, "ChargeStation.Group") ?? throw new ArgumentException("Connector not found");
            var difference = newMaxCurrentInAmps - connector.MaxCurrentInAmps;
            var groupId = connector?.ChargeStation?.GroupId ?? 0;
            var sumMaxCurrentInAmps = await _groupService.GetSumOfConnectorsMaxCurrentInAmpsByGroupId(groupId);

            if (sumMaxCurrentInAmps + difference >= connector?.ChargeStation?.Group?.CapacityInAmps)
            {
                throw new InvalidOperationException("Sum of connectors' max current in amps exceeds group's capacity");
            }

            connector.MaxCurrentInAmps = newMaxCurrentInAmps;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}