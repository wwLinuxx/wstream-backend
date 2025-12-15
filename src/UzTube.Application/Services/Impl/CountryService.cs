using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models;
using UzTube.Application.Models.Country;
using UzTube.Core.Entities;
using UzTube.DataAccess.Persistence;

namespace UzTube.Application.Services.Impl;

public class CountryService(
    DatabaseContext context
) : ICountryService
{
    public async Task<PaginatedList<CountryResponseModel>> GetCountriesAsync(PageOption option)
    {
        IQueryable<Country> query = context.Countries;

        List<CountryResponseModel> countries = await query
            .Skip(option.PageSize * (option.PageNumber - 1))
            .Take(option.PageSize)
            .Select(c => new CountryResponseModel
            {
                Id = c.Id,
                Code = c.Code,
                FullName = c.FullName
            })
            .ToListAsync();

        if (countries.Count > 0)
            throw new NotFoundException("Countries not found");

        int countriesCount = context.Countries.Count();

        return PaginatedList<CountryResponseModel>.Create(
            countries,
            countriesCount,
            option.PageNumber,
            option.PageSize);
    }
}
