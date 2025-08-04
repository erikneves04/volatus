using Volatus.Domain.Entities;

namespace Volatus.Domain.View;

public abstract class UserViewModelBase
{
    public UserViewModelBase() { }

    public UserViewModelBase(User user)
    : this(user.Name, user.Email, user.Permissions.Select(e => e.Permission.Name).ToList())
    { }

    public UserViewModelBase(string name, string email, List<string> permissions)
    {
        Name = name;
        Email = email;
        Permissions = permissions;
    }

    public string Name { get; set; }
    public string Email { get; set; }
    public List<string> Permissions { get; set; }
}

public class UserViewModel : UserViewModelBase
{
    public UserViewModel() { }

    public UserViewModel(Guid id, string name, string email, List<string> permissions) 
        : base(name, email, permissions)
    {
        Id = id;
    }

    public UserViewModel(User user) : base(user)
    {
        Id = user.Id;
    }

    public Guid Id { get; set; }
}

public class UserInsertViewModel : UserViewModelBase
{
    public string Password { get; set; }
}

public class UserUpdateViewModel : UserViewModelBase
{ }

public class UserTokenViewModel
{
    public UserTokenViewModel() { }
    public UserTokenViewModel(Guid userId, List<string> permissions, string token, DateTime expiresAt)
    {
        UserId = userId;
        Permissions = permissions;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public Guid UserId { get; set; }
    public List<string> Permissions { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class LoginParams
{
    public string Email { get; set; }
    public string Password { get; set; }
}