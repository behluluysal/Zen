using Microsoft.AspNetCore.Builder;

namespace Zen.API.Endpoints;

public abstract class EndpointGroupBase
{
    public abstract void Map(WebApplication app);
}