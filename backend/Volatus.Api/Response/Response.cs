using Volatus.Domain.Enums;
using Volatus.Domain.View;

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Volatus.Api.Response;

public class Response<ResponseObject>
    where ResponseObject : class
{ 
    public Response(StatusCodeEnums statusCode, List<string> messages, ResponseObject data = null, PaginationParams pagination = null)
    {
        StatusCode = (int)statusCode;
        StatusCodeDescription = statusCode.GetDescription();
        Messages = messages;
        Data = data;
        Pagination = pagination;
    }

    public Response(int statusCode, List<string> messages, ResponseObject data = null, PaginationParams pagination = null)
    {
        StatusCode = statusCode;
        StatusCodeDescription = ((StatusCodeEnums)statusCode).GetDescription();
        Messages = messages;
        Data = data;
        Pagination = pagination;
    }

    public int StatusCode { get; set; }
    public string StatusCodeDescription { get; set; }
    public List<string> Messages { get; set; }
    public ResponseObject Data { get; set; }
    public PaginationParams Pagination { get; set; } = null;

    private static readonly string SucessMessage = "Operation performed successfully.";

    public static ActionResult<Response<ResponseObjectType>> BuildResponse<ResponseObjectType>(
        StatusCodeEnums statusCode, 
        ResponseObjectType data = null, 
        PaginationParams pagination = null, 
        List<string> messages = null)
        where ResponseObjectType : class
    {
        return FinalBuild((int)statusCode, messages, data, pagination);
    }

    public static ActionResult<Response<ResponseObjectType>> BuildResponse<ResponseObjectType>(
        int statusCode, 
        ResponseObjectType data = null, 
        PaginationParams pagination = null, 
        List<string> messages = null)
        where ResponseObjectType : class
    {
        return FinalBuild(statusCode, messages, data, pagination);
    }

    public static ActionResult<Response<NoContent>> BuildResponse(StatusCodeEnums statusCode, List<string> messages = null)
    {
        return FinalBuild<NoContent>((int)statusCode, messages);
    }

    public static ActionResult<Response<NoContent>> BuildResponse(int statusCode, List<string> messages = null)
    {
        return FinalBuild<NoContent>(statusCode, messages);
    }

    private static ActionResult<Response<ResponseType>> FinalBuild<ResponseType>(
        int statusCode, 
        List<string> messages = null, 
        ResponseType data = null, 
        PaginationParams pagination = null)
        where ResponseType : class
    {
        var anyMessage = messages != null && messages.Any();
        if (!anyMessage)
            messages = new List<string>() { SucessMessage };

        return new ActionResult<Response<ResponseType>>(new Response<ResponseType>(statusCode, messages, data, pagination));
    }
}