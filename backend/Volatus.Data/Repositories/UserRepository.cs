using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Data.Context;

namespace Volatus.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }
}

public class UserPermissionRepository : Repository<UserPermission>, IUserPermissionRepository
{
    public UserPermissionRepository(AppDbContext context) : base(context) { }
}