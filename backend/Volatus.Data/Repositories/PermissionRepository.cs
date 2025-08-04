using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Data.Context;

namespace Volatus.Data.Repositories;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(AppDbContext context) : base(context) { }
}