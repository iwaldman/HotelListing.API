using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;
using HotelListing.API.Models.Users;

namespace HotelListing.API.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // Map from Country to CreateCountryDto
        CreateMap<Country, CreateCountryDto>()
            .ReverseMap();

        // Map from Country to GetCountryDto
        CreateMap<Country, GetCountryDto>()
            .ReverseMap();

        // Map Country to UpdateCountryDto
        CreateMap<Country, UpdateCountryDto>()
            .ReverseMap();

        // Map Hotel to UpdateHotelDto
        CreateMap<Hotel, UpdateHotelDto>()
            .ReverseMap();

        // Map Hotel to CreateHotelDto
        CreateMap<Hotel, CreateHotelDto>()
            .ReverseMap();

        // Map Hotel to GetHotelDto
        CreateMap<Hotel, GetHotelDto>()
            .ReverseMap();

        // Map Hotel to HotelDto
        CreateMap<Hotel, HotelDto>()
            .ReverseMap();

        CreateMap<ApiUserDto, ApiUser>().ReverseMap();
    }
}
