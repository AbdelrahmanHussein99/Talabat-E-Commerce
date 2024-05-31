using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Products_Specs
{
    public class ProductWithCountSpecifications: BaseSpecification<Product>
    {
        public ProductWithCountSpecifications(ProductSpecParams productSpec)
            : base(P =>
                    (!productSpec.brandId.HasValue || P.BrandId == productSpec.brandId)
                    &&
                    (!productSpec.categoryId.HasValue || P.CategoryId == productSpec.categoryId))
        { 

        
        }
    }
}
