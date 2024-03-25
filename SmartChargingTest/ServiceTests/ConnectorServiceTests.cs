using AutoMapper;
using Moq;
using SmartCharging.Configurations;
using SmartChargingService.Implementations;
using SmartChargingService.Interfaces;
using SmartCharginModels.Entities;
using SmartCharginModels.Models;
using SmartCharginRepository.Interfaces;

namespace SmartChargingTest.ServiceTests
{
    public class ConnectorServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ConnectorService _connectorService;
        private readonly Mock<IGroupService> _mockGroupService;
        private readonly Mock<IChargeStationService> _mockChargeStationService;

        public ConnectorServiceTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockGroupService = new Mock<IGroupService>();
            _mockChargeStationService = new Mock<IChargeStationService>();
            _connectorService = new ConnectorService(_mapper, _mockUnitOfWork.Object,
                _mockGroupService.Object, _mockChargeStationService.Object);
        }

        [Fact]
        public async Task AddAsync_ValidInput_ReturnsConnectorViewModel()
        {
            // Arrange
            var chargeStationId = 1;
            var newMaxCurrentInAmps = 50;
            var chargeStation = new ChargeStationViewModel { Id = chargeStationId };
            _mockChargeStationService.Setup(x => x.GetByIdAsync(chargeStationId)).ReturnsAsync(chargeStation);
            var addedConnector = new Connector { Id = 1, ChargeStationId = chargeStationId, MaxCurrentInAmps = newMaxCurrentInAmps };
            _mockUnitOfWork.Setup(x => x.ConnectorRepository.AddAsync(It.IsAny<Connector>())).ReturnsAsync(addedConnector);

            // Act
            var result = await _connectorService.AddAsync(chargeStationId, newMaxCurrentInAmps);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(addedConnector.Id, result.Id);
            Assert.Equal(addedConnector.ChargeStationId, result.ChargeStationId);
            Assert.Equal(addedConnector.MaxCurrentInAmps, result.MaxCurrentInAmps);
        }

        [Fact]
        public async Task UpdateAsync_ConnectorNotFound_ThrowsArgumentException()
        {
            // Arrange
            var connectorId = 1;
            _mockUnitOfWork.Setup(x => x.ConnectorRepository.GetByIdAsync(connectorId, "ChargeStation.Group")).ReturnsAsync((Connector)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _connectorService.UpdateAsync(connectorId, 50));
        }
    }
}
