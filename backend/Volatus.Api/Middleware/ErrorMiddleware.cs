using Volatus.Domain.Enums;
using Volatus.Domain.Exceptions;
using Volatus.Domain.View;
using Volatus.Api.Response;

using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Volatus.Api.Middleware;

public class ErrorMiddleware
{
    private readonly RequestDelegate next;

    public ErrorMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        Error errorResponseVm = null;
        
        if (ex is BaseException auxException)
        {
            // Used for known exceptions
            errorResponseVm = new Error(auxException.StatusCode, auxException.Messages);
            context.Response.StatusCode = (int)auxException.StatusCode;
        }
        else
        {
            // Used for unknown exceptions
            var message = new List<string>() { "Internal Server Error." };
            errorResponseVm = new Error(StatusCodeEnums.InternalServerError, message);
            context.Response.StatusCode = (int)StatusCodeEnums.InternalServerError;

            // Don't re-throw the exception, just log it
            // TODO: Save log to fix
        }

        context.Response.ContentType = "application/json";
        var result = JsonConvert.SerializeObject(errorResponseVm);
        return context.Response.WriteAsync(result);
    }
}