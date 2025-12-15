using UzTube.Application.Models;
using UzTube.Application.Models.Country;

namespace UzTube.Application.Services;

public interface ICountryService
{
    Task<PaginatedList<CountryResponseModel>> GetCountriesAsync(PageOption option);
}
