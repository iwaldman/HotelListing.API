using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repositories
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        /// <summary>
        /// HotelListingDbContext
        /// </summary>
        private readonly HotelListingDbContext _hotelListingDbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hotelListingDbContext"></param>
        public GenericRepository(HotelListingDbContext hotelListingDbContext)
        {
            _hotelListingDbContext = hotelListingDbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _hotelListingDbContext.AddAsync(entity);
            await _hotelListingDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                return;
            }

            var entity = await GetAsync(id);

            if (entity is null)
            {
                return;
            }

            _hotelListingDbContext.Set<T>().Remove(entity);
            await _hotelListingDbContext.SaveChangesAsync();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _hotelListingDbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int? id)
        {
            if (id is null)
            {
                return null;
            }

            return await _hotelListingDbContext.Set<T>().FindAsync(id);
        }

        public async Task<bool> IsExists(int? id)
        {
            var entity = await GetAsync(id);
            return entity is not null;
        }

        public async Task UpdateAsync(T entity)
        {
            _hotelListingDbContext.Update(entity);
            await _hotelListingDbContext.SaveChangesAsync();
        }
    }
}
