using UzTube.API.Endpoints;

namespace UzTube.API.Extensions;

public static class ApiExtensions
{
    public static void AddMappedExtensions(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        app.MapCommentEndpoints();
        app.MapViewEndpoints();
        app.MapLikeEndpoints();
        app.MapCategoryEndpoints();
        app.MapCountryEndpoints();
    }
}