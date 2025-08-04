using System.ComponentModel;
using Volatus.Domain.Enums;

namespace Volatus.Domain.Enums;

public enum StatusCodeEnums
{
    [Description("Null")]
    Null = 0,

    // informative answers
    [Description("Continue")]
    Continue = 100,
    [Description("Switching Protocol")]
    SwitchingProtocol = 101,
    [Description("Processing")]
    Processing = 102,
    [Description("Early Hints")]
    EarlyHints = 103,

    // success answers
    [Description("Ok")]
    Ok = 200,
    [Description("Created")]
    Created = 201,
    [Description("Accepted")]
    Accepted = 202,
    [Description("Non-Authoritative Information")]
    NonAuthoritativeInformation = 203,
    [Description("No Content")]
    NoContent = 204,
    [Description("Reset Content")]
    ResetContent = 205,
    [Description("Partial Content")]
    PartialContent = 206,
    [Description("Multi-Status")]
    MultiStatus = 207,

    // redirect messages
    [Description("Multiple Choice")]
    MultipleChoice = 300,
    [Description("Moved Permanently")]
    MovedPermanently = 301,
    [Description("Found")]
    Found = 302,
    [Description("See Other")]
    SeeOther = 303,
    [Description("Not Modified")]
    NotModified = 304,
    [Description("Use Proxy")]
    UseProxy = 305,
    [Description("Unused")]
    Unused = 306,
    [Description("Temporary Redirect")]
    TemporaryRedirect = 307,
    [Description("Permanent Redirect")]
    PermanentRedirect = 308,

    // Customer error responses
    [Description("Bad Request")]
    BadRequest = 400,
    [Description("Unauthorized")]
    Unauthorized = 401,
    [Description("Payment Required ")]
    PaymentRequired = 402,
    [Description("Forbidden")]
    Forbidden = 403,
    [Description("Not Found")]
    NotFound = 404,
    [Description("Method Not Allowed")]
    MethodNotAllowed = 405,
    [Description("Not Acceptable")]
    NotAcceptable = 406,
    [Description("Proxy Authentication Required")]
    ProxyAuthenticationRequired = 407,
    [Description("Request Timeout")]
    RequestTimeout = 408,
    [Description("Conflict")]
    Conflict = 409,
    [Description("Gone")]
    Gone = 410,
    [Description("Unsupported Media Type")]
    UnsupportedMediaType = 415,
    [Description("Locked")]
    Locked = 423,
    [Description("Too Many Requests")]
    TooManyRequests = 429,

    // Server error responses
    [Description("Internal Server Error")]
    InternalServerError = 500,
    [Description("Not Implemented")]
    NotImplemented = 501,
    [Description("Bad Gateway ")]
    BadGateway = 502,
    [Description("Service Unavailable ")]
    ServiceUnavailable = 503,
}