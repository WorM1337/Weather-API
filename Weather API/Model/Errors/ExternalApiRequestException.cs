using System.Net;

namespace Weather_API.Model.Errors;

public class ExternalApiRequestException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ExternalApiRequestException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
    public ExternalApiRequestException(string message) : base(message) { }
}