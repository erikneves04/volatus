using System.Linq.Expressions;
using Volatus.Domain.Entities;

namespace Volatus.Domain.View;

public abstract class PermissionBaseViewModel 
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class PermissionViewModel : PermissionBaseViewModel
{
    public Guid Id { get; set; }

    public PermissionViewModel() { }
    public PermissionViewModel(Permission permission)
    {
        Id = permission.Id;
        Name = permission.Name;
        Description = permission.Description;
    }

    public static Expression<Func<Permission, PermissionViewModel>> Converter =>
        permission => new PermissionViewModel()
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description
        };
}

public class PermissonCreateUpdateViewModel : PermissionBaseViewModel
{ }