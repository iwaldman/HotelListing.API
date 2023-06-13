using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repositories;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly HotelListingDbContext _hotelListingDbContext;

    public CountriesRepository(HotelListingDbContext hotelListingDbContext)
        : base(hotelListingDbContext)
    {
        _hotelListingDbContext = hotelListingDbContext;
    }

    public async Task<Country?> GetDetailsAsync(int id)
    {
        if (_hotelListingDbContext.Countries is null)
        {
            return null;
        }

        return await _hotelListingDbContext.Countries
            .Include(c => c.Hotels)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
