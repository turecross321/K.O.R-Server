using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;

namespace K.O.R_Server.Endpoints;

public class ApiEndpointAttribute : HttpEndpointAttribute
{
    public const string BaseRoute = "/api/v1/";
    
    public ApiEndpointAttribute(string route, HttpMethods method = HttpMethods.Get, ContentType contentType = ContentType.Plaintext)
        : base(BaseRoute + route, method, contentType)
    {}

    public ApiEndpointAttribute(string route, ContentType contentType, HttpMethods method = HttpMethods.Get) 
        : base(BaseRoute + route, contentType, method)
    {}
}