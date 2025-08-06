using Microsoft.Extensions.Logging;
using Moq;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Services;
using Xunit;

namespace Volatus.Tests;

public class DeliveryWorkerServiceTests
{
    private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;
    private readonly Mock<IDroneRepository> _mockDroneRepository;
    private readonly Mock<ILogger<DeliveryWorkerService>> _mockLogger;
    private readonly DeliveryWorkerService _service;

    public DeliveryWorkerServiceTests()
    {
        _mockDeliveryRepository = new Mock<IDeliveryRepository>();
        _mockDroneRepository = new Mock<IDroneRepository>();
        _mockLogger = new Mock<ILogger<DeliveryWorkerService>>();
        _service = new DeliveryWorkerService(_mockDeliveryRepository.Object, _mockDroneRepository.Object, _mockLogger.Object);
    }

    #region CalculateDistance Tests

    [Theory]
    [InlineData(0, 0, 0, 0, 0)] // Mesmo ponto
    [InlineData(0, 0, 3, 4, 5)] // Triângulo 3-4-5
    [InlineData(1, 1, 4, 5, 5)] // Triângulo 3-4-5 com offset
    [InlineData(-3, -4, 0, 0, 5)] // Coordenadas negativas
    [InlineData(10, 10, 13, 14, 5)] // Valores maiores
    public void CalculateDistance_ValidCoordinates_ReturnsCorrectDistance(double x1, double y1, double x2, double y2, double expectedDistance)
    {
        // Act
        var result = _service.CalculateDistance(x1, y1, x2, y2);

        // Assert
        Assert.Equal(expectedDistance, result, 2);
    }

    [Theory]
    [InlineData(double.MaxValue, 0, 0, 0)]
    [InlineData(0, double.MaxValue, 0, 0)]
    [InlineData(0, 0, double.MaxValue, 0)]
    [InlineData(0, 0, 0, double.MaxValue)]
    public void CalculateDistance_ExtremeValues_HandlesOverflow(double x1, double y1, double x2, double y2)
    {
        // Act & Assert
        var exception = Assert.Throws<OverflowException>(() => _service.CalculateDistance(x1, y1, x2, y2));
        Assert.NotNull(exception);
    }

    [Theory]
    [InlineData(double.NaN, 0, 0, 0)]
    [InlineData(0, double.NaN, 0, 0)]
    [InlineData(0, 0, double.NaN, 0)]
    [InlineData(0, 0, 0, double.NaN)]
    public void CalculateDistance_NaNValues_ReturnsNaN(double x1, double y1, double x2, double y2)
    {
        // Act
        var result = _service.CalculateDistance(x1, y1, x2, y2);

        // Assert
        Assert.True(double.IsNaN(result));
    }

    #endregion

    #region CalculateOptimalRoute Tests

    [Fact]
    public void CalculateOptimalRoute_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var deliveries = new List<Delivery>();

        // Act
        var result = _service.CalculateOptimalRoute(deliveries, 0, 0);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void CalculateOptimalRoute_SingleDelivery_ReturnsSameDelivery()
    {
        // Arrange
        var delivery = new Delivery("Test", "(5,5)", "Test delivery", 1.0, "Pendente", "Média");
        var deliveries = new List<Delivery> { delivery };

        // Act
        var result = _service.CalculateOptimalRoute(deliveries, 0, 0);

        // Assert
        Assert.Single(result);
        Assert.Equal(delivery, result[0]);
    }

    [Fact]
    public void CalculateOptimalRoute_MultipleDeliveries_ReturnsNearestNeighborRoute()
    {
        // Arrange
        var delivery1 = new Delivery("Test1", "(1,1)", "Test delivery 1", 1.0, "Pendente", "Média");
        var delivery2 = new Delivery("Test2", "(5,5)", "Test delivery 2", 1.0, "Pendente", "Média");
        var delivery3 = new Delivery("Test3", "(10,10)", "Test delivery 3", 1.0, "Pendente", "Média");
        var deliveries = new List<Delivery> { delivery2, delivery1, delivery3 }; // Not in optimal order

        // Act
        var result = _service.CalculateOptimalRoute(deliveries, 0, 0);

        // Assert
        Assert.Equal(3, result.Count);
        // Should start with the nearest delivery (1,1)
        Assert.Equal(delivery1, result[0]);
    }

    [Fact]
    public void CalculateOptimalRoute_StartingFromDifferentPoint_CalculatesCorrectly()
    {
        // Arrange
        var delivery1 = new Delivery("Test1", "(1,1)", "Test delivery 1", 1.0, "Pendente", "Média");
        var delivery2 = new Delivery("Test2", "(5,5)", "Test delivery 2", 1.0, "Pendente", "Média");
        var deliveries = new List<Delivery> { delivery1, delivery2 };

        // Act
        var result = _service.CalculateOptimalRoute(deliveries, 10, 10);

        // Assert
        Assert.Equal(2, result.Count);
        // Should start with the nearest delivery from (10,10)
        Assert.Equal(delivery2, result[0]); // (5,5) is closer to (10,10) than (1,1)
    }

    #endregion

    #region HasSufficientBatteryForDelivery Tests

    [Fact]
    public void HasSufficientBatteryForDelivery_SufficientBattery_ReturnsTrue()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 0,
            CurrentY = 0,
            CurrentBattery = 100.0
        };
        var delivery = new Delivery("Test", "(3,4)", "Test delivery", 1.0, "Pendente", "Média");

        // Act
        var result = _service.HasSufficientBatteryForDelivery(drone, delivery);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasSufficientBatteryForDelivery_InsufficientBattery_ReturnsFalse()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 0,
            CurrentY = 0,
            CurrentBattery = 5.0 // Very low battery
        };
        var delivery = new Delivery("Test", "(10,10)", "Test delivery", 1.0, "Pendente", "Média");

        // Act
        var result = _service.HasSufficientBatteryForDelivery(drone, delivery);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasSufficientBatteryForDelivery_DroneAtBase_DeliveryNearby_ReturnsTrue()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 0,
            CurrentY = 0,
            CurrentBattery = 50.0
        };
        var delivery = new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Pendente", "Média");

        // Act
        var result = _service.HasSufficientBatteryForDelivery(drone, delivery);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasSufficientBatteryForDelivery_DroneFarFromBase_DeliveryFar_ReturnsFalse()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 50,
            CurrentY = 50,
            CurrentBattery = 30.0
        };
        var delivery = new Delivery("Test", "(100,100)", "Test delivery", 1.0, "Pendente", "Média");

        // Act
        var result = _service.HasSufficientBatteryForDelivery(drone, delivery);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasSufficientBatteryForDelivery_ZeroBattery_ReturnsFalse()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
        {
            CurrentX = 0,
            CurrentY = 0,
            CurrentBattery = 0.0
        };
        var delivery = new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Pendente", "Média");

        // Act
        var result = _service.HasSufficientBatteryForDelivery(drone, delivery);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region ProcessDeliverySystemAsync Tests

    [Fact]
    public async Task ProcessDeliverySystemAsync_SuccessfulExecution_ReturnsSummary()
    {
        // Arrange
        var deliveries = new List<Delivery>();
        var drones = new List<Drone>();
        _mockDeliveryRepository.Setup(x => x.Get(null)).Returns(deliveries);
        _mockDroneRepository.Setup(x => x.Get(null)).Returns(drones);

        // Act
        var result = await _service.ProcessDeliverySystemAsync();

        // Assert
        Assert.Contains("Allocated", result);
        Assert.Contains("moved", result);
        _mockDeliveryRepository.Verify(x => x.Get(null), Times.Once);
        _mockDroneRepository.Verify(x => x.Get(null), Times.Once);
    }

    [Fact]
    public async Task ProcessDeliverySystemAsync_ExceptionThrown_PropagatesException()
    {
        // Arrange
        _mockDeliveryRepository.Setup(x => x.Get(null)).Throws(new InvalidOperationException("Test exception"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ProcessDeliverySystemAsync());
    }

    #endregion

    #region AllocateDeliveries Tests

    [Fact]
    public void AllocateDeliveries_NoPendingDeliveries_ReturnsZero()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Entregue", "Média")
        };
        _mockDeliveryRepository.Setup(x => x.Get(null)).Returns(deliveries);

        // Act
        var result = _service.AllocateDeliveries();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void AllocateDeliveries_NoAvailableDrones_ReturnsZero()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Pendente", "Média")
        };
        var drones = new List<Drone>
        {
            new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
        };
        _mockDeliveryRepository.Setup(x => x.Get(null)).Returns(deliveries);
        _mockDroneRepository.Setup(x => x.Get(null)).Returns(drones);

        // Act
        var result = _service.AllocateDeliveries();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void AllocateDeliveries_ValidAllocation_ReturnsAllocatedCount()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Pendente", "Média")
        };
        var drones = new List<Drone>
        {
            new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0)
            {
                CurrentBattery = 100.0,
                CurrentX = 0,
                CurrentY = 0
            }
        };
        _mockDeliveryRepository.Setup(x => x.Get(null)).Returns(deliveries);
        _mockDroneRepository.Setup(x => x.Get(null)).Returns(drones);

        // Act
        var result = _service.AllocateDeliveries();

        // Assert
        Assert.Equal(1, result);
        _mockDeliveryRepository.Verify(x => x.Update(It.IsAny<Delivery>()), Times.Once);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Once);
    }

    #endregion

    #region UpdateDronePositions Tests

    [Fact]
    public void UpdateDronePositions_NoActiveDrones_ReturnsZero()
    {
        // Arrange
        var drones = new List<Drone>
        {
            new Drone("Test Drone", "Model", "SN001", "Offline", 10.0, 100.0)
        };
        _mockDroneRepository.Setup(x => x.Get(null)).Returns(drones);

        // Act
        var result = _service.UpdateDronePositions();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void UpdateDronePositions_ActiveDrones_ReturnsMovedCount()
    {
        // Arrange
        var drones = new List<Drone>
        {
            new Drone("Test Drone", "Model", "SN001", "Em Uso", 10.0, 100.0)
            {
                CurrentX = 0,
                CurrentY = 0,
                TargetX = 5,
                TargetY = 5,
                Speed = 1.0
            }
        };
        _mockDroneRepository.Setup(x => x.Get(null)).Returns(drones);

        // Act
        var result = _service.UpdateDronePositions();

        // Assert
        Assert.Equal(1, result);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.AtLeastOnce);
    }

    #endregion
} 