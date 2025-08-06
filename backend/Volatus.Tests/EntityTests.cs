using Volatus.Domain.Entities;
using Xunit;

namespace Volatus.Tests;

public class EntityTests
{
    #region Delivery Tests

    [Fact]
    public void Delivery_Constructor_ValidParameters_CreatesDelivery()
    {
        // Arrange & Act
        var delivery = new Delivery("João Silva", "(10,20)", "Pacote pequeno", 2.5, "Pendente", "Média");

        // Assert
        Assert.Equal("João Silva", delivery.CustomerName);
        Assert.Equal("(10,20)", delivery.CustomerAddress);
        Assert.Equal("Pacote pequeno", delivery.Description);
        Assert.Equal(2.5, delivery.Weight);
        Assert.Equal("Pendente", delivery.Status);
        Assert.Equal("Média", delivery.Priority);
        Assert.Equal(10, delivery.X);
        Assert.Equal(20, delivery.Y);
    }

    [Theory]
    [InlineData("(0,0)", 0, 0)]
    [InlineData("(10,20)", 10, 20)]
    [InlineData("(-5,15)", -5, 15)]
    [InlineData("(100.5,200.7)", 100.5, 200.7)]
    [InlineData("(0.1,0.2)", 0.1, 0.2)]
    public void Delivery_ParseAddressToCoordinates_ValidFormats_ParsesCorrectly(string address, double expectedX, double expectedY)
    {
        // Arrange & Act
        var delivery = new Delivery("Test", address, "Test delivery", 1.0, "Pendente", "Média");

        // Assert
        Assert.Equal(expectedX, delivery.X);
        Assert.Equal(expectedY, delivery.Y);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("(10)")]
    [InlineData("(10,20,30)")]
    [InlineData("10,20")]
    [InlineData("(abc,def)")]
    public void Delivery_ParseAddressToCoordinates_InvalidFormats_HandlesGracefully(string address)
    {
        // Arrange & Act
        var delivery = new Delivery("Test", address, "Test delivery", 1.0, "Pendente", "Média");

        // Assert
        Assert.Equal(0, delivery.X);
        Assert.Equal(0, delivery.Y);
    }

    [Theory]
    [InlineData("Pendente")]
    [InlineData("Em Progresso")]
    [InlineData("Entregue")]
    [InlineData("Cancelado")]
    public void Delivery_ValidStatus_AcceptsValidStatus(string status)
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "(1,1)", "Test delivery", 1.0, status, "Média");

        // Assert
        Assert.Equal(status, delivery.Status);
    }

    [Theory]
    [InlineData("Alta")]
    [InlineData("Média")]
    [InlineData("Baixa")]
    public void Delivery_ValidPriority_AcceptsValidPriority(string priority)
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "(1,1)", "Test delivery", 1.0, "Pendente", priority);

        // Assert
        Assert.Equal(priority, delivery.Priority);
    }

    [Fact]
    public void Delivery_WeightValidation_AcceptsValidWeight()
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "(1,1)", "Test delivery", 0.1, "Pendente", "Média");

        // Assert
        Assert.Equal(0.1, delivery.Weight);
    }

    [Fact]
    public void Delivery_NegativeWeight_AcceptsNegativeWeight()
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "(1,1)", "Test delivery", -1.0, "Pendente", "Média");

        // Assert
        Assert.Equal(-1.0, delivery.Weight);
    }

    [Fact]
    public void Delivery_ZeroWeight_AcceptsZeroWeight()
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "(1,1)", "Test delivery", 0.0, "Pendente", "Média");

        // Assert
        Assert.Equal(0.0, delivery.Weight);
    }

    [Fact]
    public void Delivery_EmptyCustomerName_AcceptsEmptyName()
    {
        // Arrange & Act
        var delivery = new Delivery("", "(1,1)", "Test delivery", 1.0, "Pendente", "Média");

        // Assert
        Assert.Equal("", delivery.CustomerName);
    }

    [Fact]
    public void Delivery_EmptyDescription_AcceptsEmptyDescription()
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "(1,1)", "", 1.0, "Pendente", "Média");

        // Assert
        Assert.Equal("", delivery.Description);
    }

    #endregion

    #region Drone Tests

    [Fact]
    public void Drone_Constructor_ValidParameters_CreatesDrone()
    {
        // Arrange & Act
        var drone = new Drone("Drone Alpha", "Model X", "SN001", "Disponível", 15.0, 100.0);

        // Assert
        Assert.Equal("Drone Alpha", drone.Name);
        Assert.Equal("Model X", drone.Model);
        Assert.Equal("SN001", drone.SerialNumber);
        Assert.Equal("Disponível", drone.Status);
        Assert.Equal(15.0, drone.MaxWeight);
        Assert.Equal(100.0, drone.BatteryCapacity);
        Assert.Equal(100.0, drone.CurrentBattery);
        Assert.Equal(0, drone.CurrentX);
        Assert.Equal(0, drone.CurrentY);
        Assert.Equal(0, drone.TargetX);
        Assert.Equal(0, drone.TargetY);
        Assert.Equal(1.0, drone.Speed);
        Assert.False(drone.IsCharging);
    }

    [Theory]
    [InlineData("Disponível")]
    [InlineData("Em Uso")]
    [InlineData("Manutenção")]
    [InlineData("Offline")]
    public void Drone_ValidStatus_AcceptsValidStatus(string status)
    {
        // Arrange & Act
        var drone = new Drone("Test Drone", "Model", "SN001", status, 10.0, 100.0);

        // Assert
        Assert.Equal(status, drone.Status);
    }

    [Fact]
    public void Drone_MaxWeightValidation_AcceptsValidWeight()
    {
        // Arrange & Act
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 0.1, 100.0);

        // Assert
        Assert.Equal(0.1, drone.MaxWeight);
    }

    [Fact]
    public void Drone_ZeroMaxWeight_AcceptsZeroWeight()
    {
        // Arrange & Act
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 0.0, 100.0);

        // Assert
        Assert.Equal(0.0, drone.MaxWeight);
    }

    [Fact]
    public void Drone_NegativeMaxWeight_AcceptsNegativeWeight()
    {
        // Arrange & Act
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", -1.0, 100.0);

        // Assert
        Assert.Equal(-1.0, drone.MaxWeight);
    }

    [Fact]
    public void Drone_BatteryCapacityValidation_AcceptsValidCapacity()
    {
        // Arrange & Act
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 50.0);

        // Assert
        Assert.Equal(50.0, drone.BatteryCapacity);
    }

    [Fact]
    public void Drone_ZeroBatteryCapacity_AcceptsZeroCapacity()
    {
        // Arrange & Act
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 0.0);

        // Assert
        Assert.Equal(0.0, drone.BatteryCapacity);
    }

    [Fact]
    public void Drone_CurrentBatteryValidation_AcceptsValidBattery()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.CurrentBattery = 75.5;

        // Assert
        Assert.Equal(75.5, drone.CurrentBattery);
    }

    [Fact]
    public void Drone_CurrentBatteryAbove100_AcceptsOver100()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.CurrentBattery = 150.0;

        // Assert
        Assert.Equal(150.0, drone.CurrentBattery);
    }

    [Fact]
    public void Drone_NegativeCurrentBattery_AcceptsNegativeBattery()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.CurrentBattery = -10.0;

        // Assert
        Assert.Equal(-10.0, drone.CurrentBattery);
    }

    [Fact]
    public void Drone_PositionValidation_AcceptsValidPositions()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.CurrentX = 100.5;
        drone.CurrentY = -50.7;
        drone.TargetX = 200.0;
        drone.TargetY = 300.0;

        // Assert
        Assert.Equal(100.5, drone.CurrentX);
        Assert.Equal(-50.7, drone.CurrentY);
        Assert.Equal(200.0, drone.TargetX);
        Assert.Equal(300.0, drone.TargetY);
    }

    [Fact]
    public void Drone_SpeedValidation_AcceptsValidSpeed()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.Speed = 5.5;

        // Assert
        Assert.Equal(5.5, drone.Speed);
    }

    [Fact]
    public void Drone_ZeroSpeed_AcceptsZeroSpeed()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.Speed = 0.0;

        // Assert
        Assert.Equal(0.0, drone.Speed);
    }

    [Fact]
    public void Drone_NegativeSpeed_AcceptsNegativeSpeed()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.Speed = -2.0;

        // Assert
        Assert.Equal(-2.0, drone.Speed);
    }

    [Fact]
    public void Drone_IsChargingProperty_CanBeSet()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.IsCharging = true;

        // Assert
        Assert.True(drone.IsCharging);
    }

    [Fact]
    public void Drone_LastMovementTime_CanBeSet()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);
        var testTime = DateTime.UtcNow;

        // Act
        drone.LastMovementTime = testTime;

        // Assert
        Assert.Equal(testTime, drone.LastMovementTime);
    }

    [Fact]
    public void Drone_EmptyName_AcceptsEmptyName()
    {
        // Arrange & Act
        var drone = new Drone("", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Assert
        Assert.Equal("", drone.Name);
    }

    [Fact]
    public void Drone_EmptyModel_AcceptsEmptyModel()
    {
        // Arrange & Act
        var drone = new Drone("Test Drone", "", "SN001", "Disponível", 10.0, 100.0);

        // Assert
        Assert.Equal("", drone.Model);
    }

    [Fact]
    public void Drone_EmptySerialNumber_AcceptsEmptySerialNumber()
    {
        // Arrange & Act
        var drone = new Drone("Test Drone", "Model", "", "Disponível", 10.0, 100.0);

        // Assert
        Assert.Equal("", drone.SerialNumber);
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public void Delivery_ExtremeCoordinates_HandlesExtremeValues()
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "(999999,999999)", "Test delivery", 1.0, "Pendente", "Média");

        // Assert
        Assert.Equal(999999, delivery.X);
        Assert.Equal(999999, delivery.Y);
    }

    [Fact]
    public void Delivery_DecimalCoordinates_HandlesDecimals()
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "(3.14159,2.71828)", "Test delivery", 1.0, "Pendente", "Média");

        // Assert
        Assert.Equal(3.14159, delivery.X);
        Assert.Equal(2.71828, delivery.Y);
    }

    [Fact]
    public void Drone_ExtremePositions_HandlesExtremeValues()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.CurrentX = double.MaxValue;
        drone.CurrentY = double.MinValue;
        drone.TargetX = double.MaxValue;
        drone.TargetY = double.MinValue;

        // Assert
        Assert.Equal(double.MaxValue, drone.CurrentX);
        Assert.Equal(double.MinValue, drone.CurrentY);
        Assert.Equal(double.MaxValue, drone.TargetX);
        Assert.Equal(double.MinValue, drone.TargetY);
    }

    [Fact]
    public void Drone_ExtremeBattery_HandlesExtremeValues()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.CurrentBattery = double.MaxValue;

        // Assert
        Assert.Equal(double.MaxValue, drone.CurrentBattery);
    }

    [Fact]
    public void Drone_ExtremeSpeed_HandlesExtremeValues()
    {
        // Arrange
        var drone = new Drone("Test Drone", "Model", "SN001", "Disponível", 10.0, 100.0);

        // Act
        drone.Speed = double.MaxValue;

        // Assert
        Assert.Equal(double.MaxValue, drone.Speed);
    }

    [Fact]
    public void Delivery_WhitespaceAddress_HandlesWhitespace()
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "   (10,20)   ", "Test delivery", 1.0, "Pendente", "Média");

        // Assert
        Assert.Equal(10, delivery.X);
        Assert.Equal(20, delivery.Y);
    }

    [Fact]
    public void Delivery_AddressWithSpaces_HandlesSpaces()
    {
        // Arrange & Act
        var delivery = new Delivery("Test", "( 10 , 20 )", "Test delivery", 1.0, "Pendente", "Média");

        // Assert
        Assert.Equal(10, delivery.X);
        Assert.Equal(20, delivery.Y);
    }

    #endregion
} 