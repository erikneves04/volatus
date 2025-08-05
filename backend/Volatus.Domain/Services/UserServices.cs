using Volatus.Domain.View;
using Volatus.Domain.Entities;
using Volatus.Domain.Exceptions;
using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Volatus.Domain.Interfaces.Services.Specials;

namespace Volatus.Domain.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _repository;
    private readonly IUserPermissionRepository _userPermissionRepository;
    private readonly IPermissionServices _permissionServices;
    private readonly IPasswordServices _passwordServices;
    private readonly IEventServices _eventServices;

    public UserServices(IUserRepository repository, IUserPermissionRepository userPermissionRepository, IPermissionServices permissionServices, IPasswordServices passwordServices, IEventServices eventServices)
    {
        _repository = repository;
        _userPermissionRepository = userPermissionRepository;
        _permissionServices = permissionServices;
        _passwordServices = passwordServices;
        _eventServices = eventServices;
    }

    public IEnumerable<UserViewModel> View(PaginationParams @params)
    {
        var users = _repository.ExecuteQuery(_repository.Query(), @params);
        return users.Select(user => ConvertToViewModel(user)).ToList();
    }

    public UserViewModel View(Guid id)
    {
        var user = Get(id);
        ThrowIfNull(user, "User");

        return ConvertToViewModel(user);
    }

    public UserViewModel Insert(UserInsertViewModel model)
    {
        ThrowIfModelIsInvalid(model);

        var emailIsUsed = Exists(model.Email);
        if (emailIsUsed)
            throw new BadRequestException("Email is used by another account.");
    
        model.Password = _passwordServices.Hash(model.Password);

        var user = ConvertToEntity(model);
        _repository.Insert(user);
       
       // UpsertPermissions(user, model.Permissions);

        // Criar evento
        _eventServices.CreateEvent("Novo Usuário Cadastrado", $"Usuário '{user.Name}' ({user.Email}) foi cadastrado no sistema.");

        return ConvertToViewModel(Get(user.Id));
    }

    public UserViewModel Update(UserUpdateViewModel model, Guid id)
    {
        ThrowIfModelIsInvalid(model);

        var user = Get(id);
        ThrowIfNull(user, "User");

        if (Exists(model.Email) && model.Email != user.Email)
            throw new BadRequestException("Email is used by another account.");

        user.Name = model.Name;
        user.Email = model.Email;

        _repository.Update(user);
        UpsertPermissions(user, model.Permissions);

        // Criar evento
        _eventServices.CreateEvent("Usuário Atualizado", $"Usuário '{user.Name}' ({user.Email}) foi atualizado no sistema.");

        return ConvertToViewModel(Get(user.Id));
    }

    public void Delete(Guid id)
    {
        var user = Get(id);

        ThrowIfNull(user, "User");

        // Criar evento antes de deletar
        _eventServices.CreateEvent("Usuário Removido", $"Usuário '{user.Name}' ({user.Email}) foi removido do sistema.");

        UpsertPermissions(user, new List<string>());
        _repository.Delete(user);
    }

    private void UpsertPermissions(User user, List<string> permissions)
    {
        var permissionIds = permissions.Select(e => _permissionServices.GetIdByName(e)).ToList();
        var permissionToDelete = user.Permissions.Where(e => !permissionIds.Contains(e.PermissionId)).ToList();
        var permissionToInsert = permissionIds.Where(e => !user.Permissions.Any(p => p.PermissionId == e)).Select(e => new UserPermission(user.Id, e)).ToList();

        _userPermissionRepository.DeleteMany(permissionToDelete);
        _userPermissionRepository.InsertMany(permissionToInsert);
    }

    private bool Exists(string email)
    {
        return _repository.Query().Where(e => e.Email == email).Any();
    }

    private User Get(Guid id)
    {
        return _repository
                .Query()
                .Include(e => e.Permissions)
                .ThenInclude(e => e.Permission)
                .Where(e => e.Id == id)
                .FirstOrDefault();
    }

    private User Get(string email)
    {
        return _repository
                .Query()
                .Include(e => e.Permissions)
                .ThenInclude(e => e.Permission)
                .Where(e => e.Email == email)
                .FirstOrDefault();
    }

    private static void ThrowIfNull(object entity, string message = "Entity")
    {
        if (entity is null)
            throw new NotFoundException(message);
    }

    private static void ThrowIfModelIsInvalid(UserViewModelBase model)
    {
        var messages = new List<string>();

        if (string.IsNullOrEmpty(model.Name))
            messages.Add("Name is required.");

        if (string.IsNullOrEmpty(model.Email))
            messages.Add("Email is required.");
    }

    private static User ConvertToEntity(UserInsertViewModel model)
    {
        return new User(model.Name, model.Email, model.Password);
    }

    private static UserViewModel ConvertToViewModel(User user)
    {
        return new UserViewModel(user);
    }
}