using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        /// <summary>
        /// Returns all entities as IQueryable for further querying.
        /// </summary>
        IQueryable<T> GetAllAsQueryable();

        /// <summary>
        /// Asynchronously retrieves all entities as a list.
        /// </summary>
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves an entity by its primary key.
        /// </summary>
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously inserts a new entity.
        /// </summary>
        Task InsertAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates an existing entity.
        /// </summary>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes an entity by its primary key.
        /// </summary>
        Task DeleteAsync(object id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Asynchronously insert bulk entity by its primary key.
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously saves all changes to the database.
        /// </summary>
        Task SaveAsync(CancellationToken cancellationToken = default);
    }

}
