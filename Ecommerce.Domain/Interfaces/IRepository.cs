using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        T GetFirstOrDefault(Expression<Func<T, bool>> predicate, string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null,  string? includeProperties = null);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
