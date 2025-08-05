using System.Linq;

using Volatus.Domain.View;
using Volatus.Domain.Entities;
using Volatus.Domain.Exceptions;
using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.Interfaces.Repositories;

namespace Volatus.Domain.Services;

public class PermissionServices : IPermissionServices
{
    private readonly IPermissionRepository _repository;

    public PermissionServices(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<PermissionViewModel> View(PaginationParams @params)
    {
        return _repository
                .ExecuteQuery(_repository.Query(), @params, PermissionViewModel.Converter, permission => permission.CreatedAt)
                .ToList();
    }

    public PermissionViewModel View(Guid id)
    {
        var entity = Get(id);
        ThrowIfNull(entity);

        return new PermissionViewModel(entity);
    }

    public PermissionViewModel Insert(PermissonCreateUpdateViewModel model)
    {
        ThrowIfModelIsInvalid(model);

        var entity = ConvertToEntity(model);
        _repository.Insert(entity);

        return new PermissionViewModel(entity);
    }

    public PermissionViewModel Update(PermissonCreateUpdateViewModel model, Guid id)
    {
        ThrowIfModelIsInvalid(model);

        var entity = Get(id);
        ThrowIfNull(entity);

        _repository.Update(entity);

        return new PermissionViewModel(entity);
    }

    public void Delete(Guid id)
    {
        var entity = Get(id);

        ThrowIfNull(entity);

        if (PermissionIsUsed(id))
            throw new BadRequestException("This permission is being used.");

        _repository.Delete(entity);
    }

    public Guid GetIdByName(string name)
    {
        var id = _repository.Query().Where(e => e.Name == name).Select(e => e.Id).FirstOrDefault();
        ThrowIfNull(id, "Permission");
        return id;
    }

    private bool Exist(string name)
    {
        return _repository.Get(e => e.Name == name).Any();
    }

    private Permission Get(Guid id)
    {
        return _repository
                .Query()
                .Where(x => x.Id == id)
                .FirstOrDefault();
    }

    private bool PermissionIsUsed(Guid id)
    {
        return _repository.Query()
                .Where(x => x.Id == id)
                .Where(x => x.Users.Any())
                .FirstOrDefault() != null;
    }

    private static void ThrowIfModelIsInvalid(PermissonCreateUpdateViewModel model)
    {
        ThrowIfNull(model, "Model");

        var messages = new List<string>();

        if (string.IsNullOrEmpty(model.Name))
            messages.Add("Name is required.");

        if (string.IsNullOrEmpty(model.Description))
            messages.Add("Description is required.");

        if (messages.Any())
            throw new BadRequestException(messages);
    }

    private static void ThrowIfNull(object entity, string message = "Entity")
    {
        if (entity is null)
            throw new NotFoundException(message);
    }

    private static Permission ConvertToEntity(PermissonCreateUpdateViewModel model)
    {
        return new Permission(model.Name, model.Description);
    }
}