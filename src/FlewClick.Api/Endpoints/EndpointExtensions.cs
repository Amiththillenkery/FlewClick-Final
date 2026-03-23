using System.Reflection;

namespace FlewClick.Api.Endpoints;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpointGroups(this IEndpointRouteBuilder app)
    {
        var endpointGroupTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false }
                        && typeof(IEndpointGroup).IsAssignableFrom(t));

        foreach (var type in endpointGroupTypes)
        {
            var instance = (IEndpointGroup)Activator.CreateInstance(type)!;
            instance.MapEndpoints(app);
        }

        return app;
    }
}
