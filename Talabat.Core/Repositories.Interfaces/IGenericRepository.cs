using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);


       Task<IReadOnlyList<T>> GetAllAsync();



        Task<T?> GetByIdWithSpacAsync(ISpecifications<T> Spec);


        Task<IReadOnlyList<T>> GetAllWithSpacAsync(ISpecifications<T> Spec);

        Task<int> GetCountAsync(ISpecifications<T> Spec);

        Task AddAsync(T Item);
        void Delete(T Item);
        void update(T Item);

    }
}
