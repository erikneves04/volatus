using Volatus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volatus.Domain.Entities;

public class Permission : Entity
{
    public Permission() { }
    public Permission(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<UserPermission> Users { get; set; }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(150).IsRequired();
    }
}