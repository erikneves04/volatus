using Volatus.Domain.View;

namespace Volatus.Domain.Interfaces.Services;

public interface IUserServices
{
    IEnumerable<UserViewModel> View(PaginationParams @params);
    UserViewModel View(Guid id);
    UserViewModel Insert(UserInsertViewModel model);
    UserViewModel Update(UserUpdateViewModel model, Guid id);
    void Delete(Guid id);
}