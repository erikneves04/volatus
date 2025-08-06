using Microsoft.Extensions.Logging;
using Moq;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Services;
using Xunit;

namespace Volatus.Tests;

public class DroneMovementServiceTests
{
    private readonly Mock<IDroneRepository> _mockDroneRepository;
    private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;
    private readonly Mock<ILogger<DroneMovementService>> _mockLogger;
    private readonly DroneMovementService _service;

    public DroneMovementServiceTests()
    {
        _mockDroneRepository = new Mock<IDroneRepository>();
        _mockDeliveryRepository = new Mock<IDeliveryRepository>();
        _mockLogger = new Mock<ILogger<DroneMovementService>>();
        _service = new DroneMovementService(_mockDroneRepository.Object, _mockDeliveryRepository.Object, _mockLogger.Object);
    }

    #region HasReachedTarget Tests

    [Fact]
    public void HasReachedTarget_DroneAtTarget_ReturnsTrue()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
        {
            CurrentX = 5.0,
            CurrentY = 5.0,
            TargetX = 5.0,
            TargetY = 5.0
        };

        // Act
        var result = _service.HasReachedTarget(drone);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasReachedTarget_DroneNearTarget_ReturnsTrue()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
        {
            CurrentX = 5.05,
            CurrentY = 5.05,
            TargetX = 5.0,
            TargetY = 5.0
        };

        // Act
        var result = _service.HasReachedTarget(drone);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasReachedTarget_DroneFarFromTarget_ReturnsFalse()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
        {
            CurrentX = 0.0,
            CurrentY = 0.0,
            TargetX = 5.0,
            TargetY = 5.0
        };

        // Act
        var result = _service.HasReachedTarget(drone);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region CalculateBatteryConsumption Tests

    [Theory]
    [InlineData(0.0, 0.0)] // Sem movimento
    [InlineData(1.0, 1.0)] // Movimento de 1 unidade
    [InlineData(5.0, 5.0)] // Movimento de 5 unidades
    [InlineData(10.0, 10.0)] // Movimento de 10 unidades
    public void CalculateBatteryConsumption_ValidDistance_ReturnsCorrectConsumption(double distance, double expectedConsumption)
    {
        // Act
        var result = _service.CalculateBatteryConsumption(distance);

        // Assert
        Assert.Equal(expectedConsumption, result, 2);
    }

    [Fact]
    public void CalculateBatteryConsumption_NegativeDistance_ReturnsNegativeConsumption()
    {
        // Arrange
        var distance = -5.0;

        // Act
        var result = _service.CalculateBatteryConsumption(distance);

        // Assert
        Assert.Equal(-5.0, result, 2);
    }

    #endregion

    #region MoveDroneTowardsTargetAsync Tests

    [Fact]
    public async Task MoveDroneTowardsTargetAsync_DroneReachesTarget_ReturnsTrue()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
        {
            CurrentX = 0.0,
            CurrentY = 0.0,
            TargetX = 1.0,
            TargetY = 1.0,
            Speed = 2.0,
            CurrentBattery = 100.0
        };

        // Act
        var result = await _service.MoveDroneTowardsTargetAsync(drone);

        // Assert
        Assert.True(result);
        Assert.Equal(drone.TargetX, drone.CurrentX);
        Assert.Equal(drone.TargetY, drone.CurrentY);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Once);
    }

    [Fact]
    public async Task MoveDroneTowardsTargetAsync_DroneDoesNotReachTarget_ReturnsFalse()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
        {
            CurrentX = 0.0,
            CurrentY = 0.0,
            TargetX = 10.0,
            TargetY = 10.0,
            Speed = 1.0,
            CurrentBattery = 100.0
        };

        var initialX = drone.CurrentX;
        var initialY = drone.CurrentY;

        // Act
        var result = await _service.MoveDroneTowardsTargetAsync(drone);

        // Assert
        Assert.False(result);
        Assert.True(drone.CurrentX > initialX);
        Assert.True(drone.CurrentY > initialY);
        Assert.True(drone.CurrentBattery < 100.0);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Once);
    }

    [Fact]
    public async Task MoveDroneTowardsTargetAsync_DroneBatteryDepleted_StopsAtZero()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
        {
            CurrentX = 0.0,
            CurrentY = 0.0,
            TargetX = 10.0,
            TargetY = 10.0,
            Speed = 1.0,
            CurrentBattery = 1.0 // Very low battery
        };

        // Act
        var result = await _service.MoveDroneTowardsTargetAsync(drone);

        // Assert
        Assert.False(result);
        Assert.Equal(0.0, drone.CurrentBattery);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Once);
    }

    #endregion

    #region ChargeDroneAtBaseAsync Tests

    [Fact]
    public async Task ChargeDroneAtBaseAsync_DroneAtBaseLowBattery_ChargesDrone()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 0.0,
            CurrentY = 0.0,
            CurrentBattery = 50.0,
            IsCharging = false
        };

        // Act
        await _service.ChargeDroneAtBaseAsync(drone);

        // Assert
        Assert.True(drone.IsCharging);
        Assert.Equal(55.0, drone.CurrentBattery, 1);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Once);
    }

    [Fact]
    public async Task ChargeDroneAtBaseAsync_DroneAtBaseFullBattery_StopsCharging()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 0.0,
            CurrentY = 0.0,
            CurrentBattery = 100.0,
            IsCharging = true
        };

        // Act
        await _service.ChargeDroneAtBaseAsync(drone);

        // Assert
        Assert.False(drone.IsCharging);
        Assert.Equal(100.0, drone.CurrentBattery);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Once);
    }

    [Fact]
    public async Task ChargeDroneAtBaseAsync_DroneNotAtBase_DoesNotCharge()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 5.0,
            CurrentY = 5.0,
            CurrentBattery = 50.0,
            IsCharging = false
        };

        // Act
        await _service.ChargeDroneAtBaseAsync(drone);

        // Assert
        Assert.False(drone.IsCharging);
        Assert.Equal(50.0, drone.CurrentBattery);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Never);
    }

    [Fact]
    public async Task ChargeDroneAtBaseAsync_DroneAtBaseBatteryNearFull_ChargesToMax()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 0.0,
            CurrentY = 0.0,
            CurrentBattery = 98.0,
            IsCharging = true
        };

        // Act
        await _service.ChargeDroneAtBaseAsync(drone);

        // Assert
        Assert.False(drone.IsCharging);
        Assert.Equal(100.0, drone.CurrentBattery);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Once);
    }

    #endregion

    #region UpdateDronePositionsAsync Tests

    [Fact]
    public async Task UpdateDronePositionsAsync_NoActiveDrones_ReturnsZero()
    {
        // Arrange
        var drones = new List<Drone>
        {
            new Drone("Test Drone", "Model", "SN001", "Offline", 10.0, 100.0)
        };
        _mockDroneRepository.Setup(x => x.GetActiveDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.UpdateDronePositionsAsync();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task UpdateDronePositionsAsync_ActiveDrones_ReturnsMovedCount()
    {
        // Arrange
        var drones = new List<Drone>
        {
            new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
            {
                CurrentX = 0.0,
                CurrentY = 0.0,
                TargetX = 5.0,
                TargetY = 5.0,
                Speed = 1.0
            }
        };
        _mockDroneRepository.Setup(x => x.GetActiveDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.UpdateDronePositionsAsync();

        // Assert
        Assert.Equal(1, result);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task UpdateDronePositionsAsync_DroneReachesTarget_HandlesDelivery()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
        {
            Id = Guid.NewGuid(),
            CurrentX = 0.0,
            CurrentY = 0.0,
            TargetX = 1.0,
            TargetY = 1.0,
            Speed = 2.0
        };
        var delivery = new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Em Progresso", "Média")
        {
            DroneId = drone.Id
        };

        var drones = new List<Drone> { drone };
        var deliveries = new List<Delivery> { delivery };

        _mockDroneRepository.Setup(x => x.GetActiveDronesAsync()).ReturnsAsync(drones);
        _mockDeliveryRepository.Setup(x => x.GetDeliveriesByDroneIdAsync(drone.Id)).ReturnsAsync(deliveries);

        // Act
        var result = await _service.UpdateDronePositionsAsync();

        // Assert
        Assert.Equal(1, result);
        Assert.Equal("Entregue", delivery.Status);
        Assert.NotNull(delivery.DeliveredDate);
        _mockDeliveryRepository.Verify(x => x.Update(It.IsAny<Delivery>()), Times.Once);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task UpdateDronePositionsAsync_ExceptionThrown_PropagatesException()
    {
        // Arrange
        _mockDroneRepository.Setup(x => x.GetActiveDronesAsync()).ThrowsAsync(new InvalidOperationException("Test exception"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateDronePositionsAsync());
    }

    #endregion
} 