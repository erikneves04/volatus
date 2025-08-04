using Volatus.Domain.View;

namespace Volatus.Domain.Interfaces.Services;

public interface IPermissionServices
{
    IEnumerable<PermissionViewModel> View(PaginationParams @params);
    PermissionViewModel View(Guid id);
    PermissionViewModel Insert(PermissonCreateUpdateViewModel model);
    PermissionViewModel Update(PermissonCreateUpdateViewModel model, Guid id);
    void Delete(Guid id);
    Guid GetIdByName(string name);
}