using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T? Find(Guid id);

        IEnumerable<T> FindAll();

        Task<T> Create(T entity);

        void Update(T entity);
        void Delete(T entity);
    }
}
