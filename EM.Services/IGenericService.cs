using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EM.Services
{
    public interface IGenericService<TDto, TEntity>
    where TDto : class
    where TEntity : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(object id);
        Task<TDto?> GetByUserIdAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TDto>> GetByUserIdAsync(Expression<Func<TEntity, bool>> predicate, bool returnIenumerable);
        Task AddAsync(TDto dto);
        Task UpdateAsync(object id, TDto dto);
        Task DeleteAsync(object id);
    }
    
}
