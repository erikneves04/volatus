using Volatus.Domain.Enums;

namespace Volatus.Domain.Exceptions;

public abstract class BaseException : Exception
{
    public BaseException() { }
    public BaseException(StatusCodeEnums statusCode, List<string> messages)
    {
        StatusCode = statusCode;
        Messages = messages;
    }

    public StatusCodeEnums StatusCode { get; protected set; } = StatusCodeEnums.Null;

    public List<string> Messages { get; protected set; }

    protected List<string> ConvertManyToResponseMessage(List<string> messages)
    {
        return messages.Select(ConvertToReponseMessage).ToList();
    }

    protected abstract string ConvertToReponseMessage(string message);
}