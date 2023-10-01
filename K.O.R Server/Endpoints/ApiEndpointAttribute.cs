using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;

namespace K.O.R_Server.Endpoints;

public class ApiEndpointAttribute : HttpEndpointAttribute
{
    public const string BaseRoute = "/api/v1/";
    
    public ApiEndpointAttribute(string route, HttpMethods method = HttpMethods.Get, ContentType contentType = ContentType.Json)
        : base(BaseRoute + route, method, contentType)
    {}
}