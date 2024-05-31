using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;
using Talabat.Repository.Specifications;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }


        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await _context.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

       

        public async Task<T?> GetByIdAsync(int id)
        {

            if (typeof(T) == typeof(Product))
            {
                return await _context.Products.Where(P => P.Id == id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T;
            }
            return await _context.Set<T>().FindAsync(id);  

        }



        public async Task<IReadOnlyList<T>> GetAllWithSpacAsync(ISpecifications<T> Spec)
        {
           return await ApplySpecifications(Spec).ToListAsync();
        }
        public async Task<T?> GetByIdWithSpacAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).CountAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> Spec) 
        {
            return SpecificationsEvaluator<T>.GetQuery(_context.Set<T>(), Spec);
        }






        public async Task AddAsync(T Item)=> await _context.Set<T>().AddAsync(Item);
       

        public  void Delete(T Item) =>  _context.Set<T>().Remove(Item);

        public void update(T Item) => _context.Set<T>().Update(Item);
    }
}
