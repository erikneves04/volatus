using Volatus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volatus.Domain.Entities;

public class UserPermission : Entity
{
    public UserPermission() { }
    public UserPermission(Guid userId, Guid permissionId)
    {
        UserId = userId;
        PermissionId = permissionId;
    }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }
}

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.HasKey(up => up.Id);

        builder.HasOne(up => up.Permission)
            .WithMany(p => p.Users)
            .HasForeignKey(up => up.PermissionId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(up => up.User)
            .WithMany(u => u.Permissions)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}