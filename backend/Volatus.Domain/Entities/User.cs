using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace Volatus.Domain.Entities;

public class User : Entity
{
    public User() { }
    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<UserPermission> Permissions { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Name).HasMaxLength(150).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(150).IsRequired();
        builder.Property(user => user.Password).HasMaxLength(250).IsRequired();
    }
}