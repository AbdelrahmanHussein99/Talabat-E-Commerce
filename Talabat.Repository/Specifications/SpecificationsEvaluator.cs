using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
    public class SpecificationsEvaluator <TEntity> where TEntity : BaseEntity
    {

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity> Spec)
        {
            var query = inputQuery;

            if(Spec.Criteira is not null)
            {
                query = query.Where(Spec.Criteira);
            }

            if(Spec.OrderBy is not null)
                query =query.OrderBy(Spec.OrderBy);


            if(Spec.OrderByDesc is not null)
                query =query.OrderByDescending(Spec.OrderByDesc);


            if (Spec.IsPaginationEnabled)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);
            }

            query = Spec.Includes.Aggregate(query, (currentQuery, includsExpressions) => currentQuery.Include(includsExpressions));

            return query;
        }
    }
}
