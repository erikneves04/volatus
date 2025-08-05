using Microsoft.EntityFrameworkCore;
using Volatus.Domain.Entities;

namespace Volatus.Data.Context;

public static class ModelConfiguration
{
    public static void ApplyModelConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
        modelBuilder.ApplyConfiguration(new DroneConfiguration());
    }
}