using AutoMapper;
using SmartChargingService.Interfaces;
using SmartCharginModels.Entities;
using SmartCharginModels.Models;
using SmartCharginRepository.Interfaces;

namespace SmartChargingService.Implementations
{
    public class ChargeStationService(IMapper mapper, IUnitOfWork unitOfWork, IGroupService groupService) : IChargeStationService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGroupService _groupService = groupService;

        public async Task<ChargeStationViewModel> AddAsync(int groupId, ChargeStationPostModel chargeStation)
        {
            if (chargeStation.Connectors?.Count > 5)
            {
                throw new ArgumentException("Charging Station can have only 5 connectors");
            }

            var group = await _groupService.GetByIdAsync(groupId) ?? throw new ArgumentException("Group not found");
            var mappedConnector = _mapper.Map<ChargeStation>(chargeStation);
            mappedConnector.GroupId = groupId;
            var result = await _unitOfWork.ChargeStationRepository.AddAsync(mappedConnector);
            return _mapper.Map<ChargeStationViewModel>(result);
        }

        public async Task<bool> RemoveAsync(int chargingStationId)
        {
            var chargeStationToRemove = await _unitOfWork.ChargeStationRepository.GetByIdAsync(chargingStationId, "Connectors") ?? throw new ArgumentException("ChargeStation not found");
            return await _unitOfWork.ChargeStationRepository.RemoveAsync(chargeStationToRemove);
        }

        public async Task<ChargeStationViewModel> GetByIdAsync(int chargingStationId)
        {
            var chargeStation = await _unitOfWork.ChargeStationRepository.GetByIdAsync(chargingStationId, "Connectors");
            return _mapper.Map<ChargeStationViewModel>(chargeStation);
        }

        public async Task<IEnumerable<ChargeStationViewModel>> GetAllAsync()
        {
            var chargeStations = await _unitOfWork.ChargeStationRepository.GetAllAsync("Connectors");
            return _mapper.Map<List<ChargeStationViewModel>>(chargeStations);
        }

        public async Task<ChargeStationViewModel> UpdateAsync(int groupId, ChargeStationViewModel chargeStation)
        {
            var mappedChargeStation = _mapper.Map<ChargeStation>(chargeStation);
            mappedChargeStation.GroupId = groupId;
            var result = await _unitOfWork.ChargeStationRepository.UpdateAsync(groupId, mappedChargeStation);
            return _mapper.Map<ChargeStationViewModel>(result);
        }
    }
}