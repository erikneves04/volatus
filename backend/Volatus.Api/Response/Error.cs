using Volatus.Domain.Enums;
using System.Collections.Generic;

namespace Volatus.Api.Response;

public class Error
{
    public Error() { }
    public Error(StatusCodeEnums statusCode, List<string> erros) 
    {
        StatusCode = statusCode;
        Erros = erros;
    }

    public StatusCodeEnums StatusCode;
    public List<string> Erros;
}