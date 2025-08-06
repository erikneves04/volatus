using Microsoft.Extensions.Logging;
using Moq;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Services;
using Xunit;

namespace Volatus.Tests;

public class DeliveryAllocationServiceTests
{
    private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;
    private readonly Mock<IDroneRepository> _mockDroneRepository;
    private readonly Mock<ILogger<DeliveryAllocationService>> _mockLogger;
    private readonly DeliveryAllocationService _service;

    public DeliveryAllocationServiceTests()
    {
        _mockDeliveryRepository = new Mock<IDeliveryRepository>();
        _mockDroneRepository = new Mock<IDroneRepository>();
        _mockLogger = new Mock<ILogger<DeliveryAllocationService>>();
        _service = new DeliveryAllocationService(_mockDeliveryRepository.Object, _mockDroneRepository.Object, _mockLogger.Object);
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

    [Fact]
    public void CalculateOptimalRoute_DeliveriesWithSameDistance_HandlesTie()
    {
        // Arrange
        var delivery1 = new Delivery("Test1", "(1,1)", "Test delivery 1", 1.0, "Pendente", "Média");
        var delivery2 = new Delivery("Test2", "(1,1)", "Test delivery 2", 1.0, "Pendente", "Média");
        var deliveries = new List<Delivery> { delivery1, delivery2 };

        // Act
        var result = _service.CalculateOptimalRoute(deliveries, 0, 0);

        // Assert
        Assert.Equal(2, result.Count);
        // Should handle ties gracefully
        Assert.Contains(delivery1, result);
        Assert.Contains(delivery2, result);
    }

    #endregion

    #region AllocateDeliveriesAsync Tests

    [Fact]
    public async Task AllocateDeliveriesAsync_NoPendingDeliveries_ReturnsZero()
    {
        // Arrange
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(new List<Delivery>());

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(0, result);
        _mockDeliveryRepository.Verify(x => x.GetPendingDeliveriesAsync(), Times.Once);
        _mockDroneRepository.Verify(x => x.GetAvailableDronesAsync(), Times.Never);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_NoAvailableDrones_ReturnsZero()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Pendente", "Média")
        };
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(deliveries);
        _mockDroneRepository.Setup(x => x.GetAvailableDronesAsync()).ReturnsAsync(new List<Drone>());

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(0, result);
        _mockDeliveryRepository.Verify(x => x.GetPendingDeliveriesAsync(), Times.Once);
        _mockDroneRepository.Verify(x => x.GetAvailableDronesAsync(), Times.Once);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_ValidAllocation_ReturnsAllocatedCount()
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
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(deliveries);
        _mockDroneRepository.Setup(x => x.GetAvailableDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(1, result);
        _mockDeliveryRepository.Verify(x => x.Update(It.IsAny<Delivery>()), Times.Once);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Once);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_PriorityOrder_AllocatesHighPriorityFirst()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new Delivery("Low Priority", "(1,1)", "Low priority delivery", 1.0, "Pendente", "Baixa"),
            new Delivery("High Priority", "(2,2)", "High priority delivery", 1.0, "Pendente", "Alta"),
            new Delivery("Medium Priority", "(3,3)", "Medium priority delivery", 1.0, "Pendente", "Média")
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
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(deliveries);
        _mockDroneRepository.Setup(x => x.GetAvailableDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(1, result);
        // Should allocate the high priority delivery first
        _mockDeliveryRepository.Verify(x => x.Update(It.Is<Delivery>(d => d.Priority == "Alta")), Times.Once);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_DroneInsufficientCapacity_DoesNotAllocate()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new Delivery("Test", "(1,1)", "Test delivery", 15.0, "Pendente", "Média") // Heavy delivery
        };
        var drones = new List<Drone>
        {
            new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0) // Max weight 10kg
            {
                CurrentBattery = 100.0,
                CurrentX = 0,
                CurrentY = 0
            }
        };
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(deliveries);
        _mockDroneRepository.Setup(x => x.GetAvailableDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(0, result);
        _mockDeliveryRepository.Verify(x => x.Update(It.IsAny<Delivery>()), Times.Never);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Never);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_DroneLowBattery_DoesNotAllocate()
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
                CurrentBattery = 10.0, // Low battery
                CurrentX = 0,
                CurrentY = 0
            }
        };
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(deliveries);
        _mockDroneRepository.Setup(x => x.GetAvailableDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(0, result);
        _mockDeliveryRepository.Verify(x => x.Update(It.IsAny<Delivery>()), Times.Never);
        _mockDroneRepository.Verify(x => x.Update(It.IsAny<Drone>()), Times.Never);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_MultipleDrones_SelectsBestDrone()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Pendente", "Média")
        };
        var drones = new List<Drone>
        {
            new Drone("Drone 1", "Model", "SN001", "Disponível", 10.0, 100.0)
            {
                CurrentBattery = 50.0,
                CurrentX = 0,
                CurrentY = 0
            },
            new Drone("Drone 2", "Model", "SN002", "Disponível", 10.0, 100.0)
            {
                CurrentBattery = 100.0, // Better battery
                CurrentX = 0,
                CurrentY = 0
            }
        };
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(deliveries);
        _mockDroneRepository.Setup(x => x.GetAvailableDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(1, result);
        // Should select the drone with better battery
        _mockDroneRepository.Verify(x => x.Update(It.Is<Drone>(d => d.CurrentBattery == 100.0)), Times.Once);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_ExceptionThrown_PropagatesException()
    {
        // Arrange
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ThrowsAsync(new InvalidOperationException("Test exception"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AllocateDeliveriesAsync());
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public void CalculateDistance_ZeroDistance_ReturnsZero()
    {
        // Act
        var result = _service.CalculateDistance(0, 0, 0, 0);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateDistance_VerySmallValues_HandlesPrecision()
    {
        // Act
        var result = _service.CalculateDistance(0.0001, 0.0001, 0.0002, 0.0002);

        // Assert
        Assert.True(result > 0);
        Assert.True(result < 0.0002);
    }

    [Fact]
    public void CalculateDistance_VeryLargeValues_HandlesScale()
    {
        // Act
        var result = _service.CalculateDistance(1000000, 1000000, 1000001, 1000001);

        // Assert
        Assert.True(result > 0);
        Assert.True(result < 2);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_DroneWithZeroBattery_DoesNotAllocate()
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
                CurrentBattery = 0.0,
                CurrentX = 0,
                CurrentY = 0
            }
        };
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(deliveries);
        _mockDroneRepository.Setup(x => x.GetAvailableDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task AllocateDeliveriesAsync_DeliveryWithZeroWeight_AllocatesSuccessfully()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new Delivery("Test", "(1,1)", "Test delivery", 0.0, "Pendente", "Média")
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
        _mockDeliveryRepository.Setup(x => x.GetPendingDeliveriesAsync()).ReturnsAsync(deliveries);
        _mockDroneRepository.Setup(x => x.GetAvailableDronesAsync()).ReturnsAsync(drones);

        // Act
        var result = await _service.AllocateDeliveriesAsync();

        // Assert
        Assert.Equal(1, result);
    }

    #endregion
} 