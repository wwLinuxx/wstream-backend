using Microsoft.AspNetCore.Mvc;
using UzTube.Application.Models;
using UzTube.Application.Models.Country;
using UzTube.Application.Services;

namespace UzTube.API.Endpoints;

public static class CountryEndpoints
{
    public static IEndpointRouteBuilder MapCountryEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/countries");

        endpoints.MapPost(string.Empty, GetCountriesAsync);

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
