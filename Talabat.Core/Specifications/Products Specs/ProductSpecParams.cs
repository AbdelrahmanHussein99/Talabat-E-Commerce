using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Products_Specs
{
    public class ProductSpecParams
    {
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        private const int MaxPageSize = 5;

        public int PageIndex { get; set; } = 1;


        private int pageSize = MaxPageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }


        public string? Sort { get; set; }

        public int? brandId { get; set; }
        public int? categoryId { get; set; }
    }
}
