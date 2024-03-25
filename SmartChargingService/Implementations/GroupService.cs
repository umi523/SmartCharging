using AutoMapper;
using SmartChargingService.Interfaces;
using SmartCharginModels.Entities;
using SmartCharginModels.Models;
using SmartCharginRepository.Interfaces;

namespace SmartChargingService.Implementations
{
    public class GroupService(IMapper mapper, IUnitOfWork unitOfWork) : IGroupService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<GroupViewModel> AddAsync(GroupPostModel group)
        {
            if (!IsValidGroup(group))
            {
                throw new Exception("Invalid group configuration");
            }

            var sumMaxCurrentInAmps = group.ChargeStations?.SelectMany(cs => cs.Connectors ?? [])
                .Sum(c => c.MaxCurrentInAmps);

            if (sumMaxCurrentInAmps >= group.CapacityInAmps)
            {
                throw new InvalidOperationException("Sum of connectors' max current in amps exceeds group's capacity");
            }

            var mappedGroupModel = _mapper.Map<Group>(group);

            return _mapper.Map<GroupViewModel>(await _unitOfWork.GroupRepository.AddAsync(mappedGroupModel));
        }

        public async Task<bool> RemoveAsync(int groupId)
        {
            var groupToRemove = await _unitOfWork.GroupRepository.GetByIdAsync(groupId, "ChargeStations.Connectors") ?? throw new ArgumentException("Group not found");

            await _unitOfWork.GroupRepository.RemoveAsync(groupToRemove);

            return true;
        }

        public async Task<GroupViewModel> GetByIdAsync(int groupId)
        {
            var group = await _unitOfWork.GroupRepository.GetByIdAsync(groupId, "ChargeStations.Connectors") ?? throw new ArgumentException("Group not found");
            return _mapper.Map<GroupViewModel>(group);
        }

        public async Task<IEnumerable<GroupViewModel>> GetAllAsync()
        {
            var groups = await _unitOfWork.GroupRepository.GetAllAsync("ChargeStations.Connectors");
            return _mapper.Map<List<GroupViewModel>>(groups);
        }

        public async Task<GroupViewModel> UpdateAsync(int groupId, Group group)
        {
            var existingGroup = await _unitOfWork.GroupRepository.GetByIdAsync(groupId, "ChargeStations.Connectors") ?? throw new ArgumentException("Group not found");

            existingGroup.Name = group.Name;
            existingGroup.CapacityInAmps = group.CapacityInAmps;

            // Create dictionaries to quickly lookup existing charge stations and connectors
            var existingChargeStations = existingGroup.ChargeStations?.ToDictionary(cs => cs.Id);
            var existingConnectors = existingGroup.ChargeStations?
                .SelectMany(cs => cs.Connectors ?? [])
                .ToDictionary(c => c.Id);

            var sumMaxCurrentInAmps = group.ChargeStations?.SelectMany(cs => cs.Connectors ?? []).Sum(c => c.MaxCurrentInAmps);

            if (sumMaxCurrentInAmps >= existingGroup.CapacityInAmps)
            {
                throw new InvalidOperationException("Sum of connectors' max current in amps exceeds group's capacity");
            }

            foreach (var updatedChargeStation in group.ChargeStations ?? [])
            {
                if (!existingChargeStations.TryGetValue(updatedChargeStation.Id, out var existingChargeStation))
                {
                    existingGroup.ChargeStations?.Add(updatedChargeStation);
                }
                else
                {
                    existingChargeStation.Name = updatedChargeStation.Name;
                }

                foreach (var updatedConnector in updatedChargeStation.Connectors ?? [])
                {
                    if (!existingConnectors.TryGetValue(updatedConnector.Id, out var existingConnector))
                    {
                        existingChargeStation?.Connectors?.Add(updatedConnector);
                    }
                    else
                    {
                        // Update connector's properties
                        existingConnector.MaxCurrentInAmps = updatedConnector.MaxCurrentInAmps;
                    }
                }
            }

            // Remove charge stations and connectors not present in the updated group
            foreach (var existingChargeStation in existingGroup.ChargeStations ?? [])
            {
                if (!group.ChargeStations.Any(cs => cs.Id == existingChargeStation.Id))
                {
                    // Remove charging station from the group
                    existingGroup?.ChargeStations?.Remove(existingChargeStation);
                }
                else
                {
                    // Remove connectors not present in the updated charging station
                    existingChargeStation.Connectors?.RemoveAll(c => !group.ChargeStations
                        .FirstOrDefault(cs => cs.Id == existingChargeStation.Id)
                        .Connectors.Any(updatedConnector => updatedConnector.Id == c.Id));
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GroupViewModel>(existingGroup);
        }

        public async Task<int> GetSumOfConnectorsMaxCurrentInAmpsByGroupId(int groupId)
        {
            var group = await _unitOfWork.GroupRepository.GetByIdAsync(groupId, "ChargeStations.Connectors");
            return group == null
                ? throw new ArgumentException("Group not found")
                : group.ChargeStations
                .SelectMany(cs => cs.Connectors ?? [])
                .Sum(c => c.MaxCurrentInAmps);
        }

        private static bool IsValidGroup(GroupPostModel group)
        {
            return group.ChargeStations == null
                   || !group.ChargeStations.Any(chargeStation => chargeStation.Connectors == null || chargeStation.Connectors.Count == 0);
        }
    }
}