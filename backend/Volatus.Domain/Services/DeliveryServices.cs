using Volatus.Domain.View;
using Volatus.Domain.Entities;
using Volatus.Domain.Exceptions;
using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.Interfaces.Repositories;

namespace Volatus.Domain.Services;

public class DeliveryServices : IDeliveryServices
{
    private readonly IDeliveryRepository _repository;
    private readonly IDroneRepository _droneRepository;
    private readonly IEventServices _eventServices;

    public DeliveryServices(IDeliveryRepository repository, IDroneRepository droneRepository, IEventServices eventServices)
    {
        _repository = repository;
        _droneRepository = droneRepository;
        _eventServices = eventServices;
    }

    public IEnumerable<DeliveryViewModel> View(PaginationParams @params)
    {
        var deliveries = _repository.ExecuteQuery(_repository.Query(), @params);
        return deliveries.Select(delivery => ConvertToViewModel(delivery)).ToList();
    }

    public DeliveryViewModel View(Guid id)
    {
        var delivery = Get(id);
        ThrowIfNull(delivery, "Delivery");

        return ConvertToViewModel(delivery);
    }

    public DeliveryViewModel Insert(DeliveryInsertViewModel model)
    {
        ThrowIfModelIsInvalid(model);

        var delivery = ConvertToEntity(model);
        _repository.Insert(delivery);

        // Criar evento
        _eventServices.CreateEvent("Nova Entrega Cadastrada", $"Entrega para '{delivery.CustomerName}' foi cadastrada no sistema.");

        return ConvertToViewModel(Get(delivery.Id));
    }

    public DeliveryViewModel Update(DeliveryUpdateViewModel model, Guid id)
    {
        ThrowIfModelIsInvalid(model);

        var delivery = Get(id);
        ThrowIfNull(delivery, "Delivery");

        delivery.CustomerName = model.CustomerName;
        delivery.CustomerAddress = model.CustomerAddress;
        delivery.Description = model.Description;
        delivery.Weight = model.Weight;
        delivery.Status = model.Status;
        delivery.DeliveredDate = model.DeliveredDate;
        delivery.Notes = model.Notes;

        _repository.Update(delivery);

        // Criar evento
        _eventServices.CreateEvent("Entrega Atualizada", $"Entrega para '{delivery.CustomerName}' foi atualizada no sistema.");

        return ConvertToViewModel(Get(delivery.Id));
    }

    public void Delete(Guid id)
    {
        var delivery = Get(id);
        ThrowIfNull(delivery, "Delivery");

        // Criar evento antes de deletar
        _eventServices.CreateEvent("Entrega Removida", $"Entrega para '{delivery.CustomerName}' foi removida do sistema.");

        _repository.Delete(delivery);
    }

    public DeliveryViewModel AssignDrone(DeliveryAssignmentViewModel model)
    {
        var delivery = Get(model.DeliveryId);
        ThrowIfNull(delivery, "Delivery");

        var drone = _droneRepository.Query().Where(d => d.Id == model.DroneId).FirstOrDefault();
        ThrowIfNull(drone, "Drone");

        // Verificar se o drone está disponível
        if (drone.Status != "Available")
            throw new BadRequestException("Drone is not available for assignment.");

        // Verificar se o peso da entrega não excede a capacidade do drone
        if (delivery.Weight > drone.MaxWeight)
            throw new BadRequestException($"Delivery weight ({delivery.Weight}kg) exceeds drone capacity ({drone.MaxWeight}kg).");

        // Atualizar a entrega com o drone
        delivery.DroneId = model.DroneId;
        delivery.Status = "InProgress";
        _repository.Update(delivery);

        // Atualizar o status do drone
        drone.Status = "InUse";
        _droneRepository.Update(drone);

        // Criar evento
        _eventServices.CreateEvent("Drone Alocado", $"Drone '{drone.Name}' foi alocado para entrega de '{delivery.CustomerName}'.");

        return ConvertToViewModel(Get(delivery.Id));
    }

    private Delivery Get(Guid id)
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

    private static void ThrowIfModelIsInvalid(DeliveryViewModelBase model)
    {
        var messages = new List<string>();

        if (string.IsNullOrEmpty(model.CustomerName))
            messages.Add("Customer name is required.");

        if (string.IsNullOrEmpty(model.CustomerAddress))
            messages.Add("Customer address is required.");

        if (string.IsNullOrEmpty(model.Description))
            messages.Add("Description is required.");

        if (model.Weight <= 0)
            messages.Add("Weight must be greater than 0.");

        if (string.IsNullOrEmpty(model.Status))
            messages.Add("Status is required.");

        if (messages.Any())
            throw new BadRequestException(string.Join(" ", messages));
    }

    private static Delivery ConvertToEntity(DeliveryInsertViewModel model)
    {
        return new Delivery(model.CustomerName, model.CustomerAddress, model.Description, model.Weight, model.Status)
        {
            DeliveredDate = model.DeliveredDate,
            Notes = model.Notes,
            DroneId = model.DroneId
        };
    }

    private static DeliveryViewModel ConvertToViewModel(Delivery delivery)
    {
        return new DeliveryViewModel(delivery);
    }
} 