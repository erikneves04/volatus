using Volatus.Domain.Enums;

namespace Volatus.Domain.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException()
    {
        StatusCode = StatusCodeEnums.NotFound;
    }

    public NotFoundException(string message)
        : this()
    {
        Messages = new List<string>() { ConvertToReponseMessage(message) };
    }
    
    public NotFoundException(List<string> messages)
        : this()
    {
        Messages = ConvertManyToResponseMessage(messages);
    }

    protected override string ConvertToReponseMessage(string message)
    {
        return $"{message} not found.";
    }
}