using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // Map from Country to CountryDTO
        CreateMap<Country, CreateCountryDto>()
            .ReverseMap();

        // Map from Country to CountryDTO
        CreateMap<Country, GetCountryDto>()
            .ReverseMap();

        // Map from Country to CountryDTO
        CreateMap<Country, CountryDto>()
            .ReverseMap();

        // Map Hotel to HotelDto
        CreateMap<Hotel, HotelDto>()
            .ReverseMap();

        // Map Country to UpdateCountryDto
        CreateMap<Country, UpdateCountryDto>()
            .ReverseMap();
    }
}
