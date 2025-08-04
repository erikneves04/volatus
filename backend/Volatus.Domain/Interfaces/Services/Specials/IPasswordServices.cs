namespace Volatus.Domain.Interfaces.Services.Specials;

public interface IPasswordServices
{
    string Hash(string password);
    bool Check(string password, string hash);
}