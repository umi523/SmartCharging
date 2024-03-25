using AutoMapper;
using Moq;
using SmartCharging.Configurations;
using SmartChargingService.Implementations;
using SmartChargingService.Interfaces;
using SmartCharginModels.Entities;
using SmartCharginRepository.Interfaces;

namespace SmartChargingTest.ServiceTests
{
    public class GroupServiceTests
    {

        private readonly IMapper _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IGroupService _groupService;

        public GroupServiceTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            _mockMapper = mockMapperConfig.CreateMapper();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _groupService = new GroupService(_mockMapper, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task UpdateAsync_ExceedsGroupCapcity_ThrowsInvalidOperationException()
        {
            // Arrange
            var groupId = 1;
            var updatedGroup = new Group
            {
                Id = groupId,
                Name = "Updated Group Name",
                CapacityInAmps = 150,
                ChargeStations =
                [
                    new ChargeStation
                    {
                        Id = 1,
                        Name = "Updated Charge Station Name",
                        Connectors =
                        [
                            new Connector { Id = 1, MaxCurrentInAmps = 30 },
                            new Connector { Id = 2, MaxCurrentInAmps = 40 }
                        ]
                    },
                    new ChargeStation
                    {
                        Id = 2,
                        Name = "New Charge Station Name",
                        Connectors =
                        [
                            new Connector { MaxCurrentInAmps = 50 },
                            new Connector { MaxCurrentInAmps = 60 }
                        ]
                    }
                ]
            };

            var existingGroup = new Group
            {
                Id = groupId,
                Name = "Existing Group Name",
                CapacityInAmps = 100,
                ChargeStations =
                [
                    new ChargeStation
                    {
                        Id = 1,
                        Name = "Existing Charge Station 1",
                        Connectors =
                        [
                            new Connector { Id = 1, MaxCurrentInAmps = 20 },
                            new Connector { Id = 2, MaxCurrentInAmps = 30 }
                        ]
                    }
                ]
            };

            _mockUnitOfWork.Setup(x => x.GroupRepository.GetByIdAsync(groupId, "ChargeStations.Connectors")).ReturnsAsync(existingGroup);

            // Act  Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _groupService.UpdateAsync(groupId, updatedGroup));
        }
    }
}
