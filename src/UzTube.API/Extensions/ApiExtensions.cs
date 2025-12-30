using UzTube.API.Endpoints;

namespace UzTube.API.Extensions;

public static class ApiExtensions
{
    public static void AddMappedExtensions(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        app.MapCommentEndpoints();
        app.MapCategoryEndpoints();
        app.MapCountryEndpoints();
    }
}