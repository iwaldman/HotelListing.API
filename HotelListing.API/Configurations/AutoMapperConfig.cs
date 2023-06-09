using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;

namespace HotelListing.API.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // map from Country to CountryDTO
        CreateMap<Country, CreateCountryDto>()
            .ReverseMap();
    }
}
