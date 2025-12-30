using Microsoft.AspNetCore.Mvc;
using UzTube.API.Extensions;
using UzTube.Application.Models;
using UzTube.Application.Models.Country;
using UzTube.Application.Services;
using UzTube.Core.Enums;

namespace UzTube.API.Endpoints;

public static class CountryEndpoints
{
    public static IEndpointRouteBuilder MapCountryEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/countries");

        endpoints.MapPost("get-countries", GetCountriesAsync);
            //.RequirePermission(SystemPermissions.CategoryView);

        return app;
    }

    private static async Task<IResult> GetCountriesAsync(
        [FromBody] PageOption option,
        [FromServices] ICountryService countryService)
    {
        return Results.Ok(ApiResult<PaginatedList<CountryResponseModel>>.Success(
            await countryService.GetCountriesAsync(option)));
    }
}
