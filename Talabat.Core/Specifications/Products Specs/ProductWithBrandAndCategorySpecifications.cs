using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;


namespace Talabat.Core.Specifications.Products_Specs
{
    public class ProductWithBrandAndCategorySpecifications :BaseSpecification<Product>
    {

        public ProductWithBrandAndCategorySpecifications(ProductSpecParams productSpec) 
            :base(P=>
                    (string.IsNullOrEmpty(productSpec.Search)||P.Name.ToLower().Contains(productSpec.Search))
                    &&
                    (!productSpec.brandId.HasValue || P.BrandId== productSpec.brandId)
                    &&
                    (!productSpec.categoryId.HasValue || P.CategoryId == productSpec.categoryId))
        {
            Includes.Add(P=>P.Brand);
            Includes.Add(P=>P.Category);
            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P=>P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P=>P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);

            }

            ApplyPagination(productSpec.PageSize *(productSpec.PageIndex - 1),productSpec.PageSize);

        }

        public ProductWithBrandAndCategorySpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
