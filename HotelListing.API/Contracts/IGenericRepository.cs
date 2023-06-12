namespace HotelListing.API.Contracts
{
    /// <summary>
    /// Generic Repository Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T>
        where T : class
    {
        Task<T?> GetAsync(int? id);

        Task<IList<T>> GetAllAsync();

        Task<bool> IsExists(int? id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int? id);
    }
}
