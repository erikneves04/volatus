using Volatus.Domain.Entities;

namespace Volatus.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{ }

public interface IUserPermissionRepository : IRepository<UserPermission>
{ }