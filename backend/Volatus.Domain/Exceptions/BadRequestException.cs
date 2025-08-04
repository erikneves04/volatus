using Volatus.Domain.Enums;

namespace Volatus.Domain.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException()
    {
        StatusCode = StatusCodeEnums.BadRequest;
    }

    public BadRequestException(string message)
        : this()
    {
        Messages = new List<string>() { ConvertToReponseMessage(message) };
    }
    
    public BadRequestException(List<string> messages)
        : this()
    {
        Messages = ConvertManyToResponseMessage(messages);
    }

    protected override string ConvertToReponseMessage(string message)
    {
        return message;
    }
}