using Volatus.Domain.View;
using Volatus.Domain.Entities;
using Volatus.Domain.Exceptions;
using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.Interfaces.Repositories;

namespace Volatus.Domain.Services;

public class DroneServices : IDroneServices
{
    private readonly IDroneRepository _repository;

    public DroneServices(IDroneRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<DroneViewModel> View(PaginationParams @params)
    {
        var drones = _repository.ExecuteQuery(_repository.Query(), @params);
        return drones.Select(drone => ConvertToViewModel(drone)).ToList();
    }

    public DroneViewModel View(Guid id)
    {
        var drone = Get(id);
        ThrowIfNull(drone, "Drone");

        return ConvertToViewModel(drone);
    }

    public DroneViewModel Insert(DroneInsertViewModel model)
    {
        ThrowIfModelIsInvalid(model);

        var serialNumberIsUsed = Exists(model.SerialNumber);
        if (serialNumberIsUsed)
            throw new BadRequestException("Serial number is already in use by another drone.");

        var drone = ConvertToEntity(model);
        _repository.Insert(drone);

        return ConvertToViewModel(Get(drone.Id));
    }

    public DroneViewModel Update(DroneUpdateViewModel model, Guid id)
    {
        ThrowIfModelIsInvalid(model);

        var drone = Get(id);
        ThrowIfNull(drone, "Drone");

        if (Exists(model.SerialNumber) && model.SerialNumber != drone.SerialNumber)
            throw new BadRequestException("Serial number is already in use by another drone.");

        drone.Name = model.Name;
        drone.Model = model.Model;
        drone.SerialNumber = model.SerialNumber;
        drone.Status = model.Status;
        drone.MaxWeight = model.MaxWeight;
        drone.BatteryCapacity = model.BatteryCapacity;
        drone.CurrentBattery = model.CurrentBattery;
        drone.Notes = model.Notes;

        _repository.Update(drone);

        return ConvertToViewModel(Get(drone.Id));
    }

    public void Delete(Guid id)
    {
        var drone = Get(id);
        ThrowIfNull(drone, "Drone");

        _repository.Delete(drone);
    }

    private bool Exists(string serialNumber)
    {
        return _repository.Query().Where(e => e.SerialNumber == serialNumber).Any();
    }

    private Drone Get(Guid id)
    {
        return _repository
                .Query()
                .Where(e => e.Id == id)
                .FirstOrDefault();
    }

    private static void ThrowIfNull(object entity, string message = "Entity")
    {
        if (entity is null)
            throw new NotFoundException(message);
    }

    private static void ThrowIfModelIsInvalid(DroneViewModelBase model)
    {
        var messages = new List<string>();

        if (string.IsNullOrEmpty(model.Name))
            messages.Add("Name is required.");

        if (string.IsNullOrEmpty(model.Model))
            messages.Add("Model is required.");

        if (string.IsNullOrEmpty(model.SerialNumber))
            messages.Add("Serial number is required.");

        if (string.IsNullOrEmpty(model.Status))
            messages.Add("Status is required.");

        if (model.MaxWeight <= 0)
            messages.Add("Max weight must be greater than 0.");

        if (model.BatteryCapacity <= 0)
            messages.Add("Battery capacity must be greater than 0.");

        if (model.CurrentBattery < 0 || model.CurrentBattery > 100)
            messages.Add("Current battery must be between 0 and 100.");

        if (messages.Any())
            throw new BadRequestException(string.Join(" ", messages));
    }

    private static Drone ConvertToEntity(DroneInsertViewModel model)
    {
        return new Drone(model.Name, model.Model, model.SerialNumber, model.Status, model.MaxWeight, model.BatteryCapacity)
        {
            CurrentBattery = model.CurrentBattery,
            Notes = model.Notes
        };
    }

    private static DroneViewModel ConvertToViewModel(Drone drone)
    {
        return new DroneViewModel(drone);
    }
} 