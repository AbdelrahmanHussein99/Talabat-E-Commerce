using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Dtos;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications.Products_Specs;
using Talabat.Repository.Specifications;

namespace Talabat.APIS.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductCategory> _categoryRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo,
            IMapper mapper,
            IGenericRepository<ProductBrand> BrandRepo,
            IGenericRepository<ProductCategory> CategoryRepo)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _brandRepo = BrandRepo;
            _categoryRepo = CategoryRepo;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productSpec)
        {
            var spec =new ProductWithBrandAndCategorySpecifications(productSpec);
            //var products = await _productRepo.GetAllAsync();
            var products = await _productRepo.GetAllWithSpacAsync(spec);
            var countSpec = new ProductWithCountSpecifications(productSpec);

            var count = await _productRepo.GetCountAsync(countSpec);

           var result = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productSpec.PageIndex,productSpec.PageSize, count, result ));
        }
        [Authorize]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            //var product = await _productRepo.GetByIdAsync(id);
            var product = await _productRepo.GetByIdWithSpacAsync(spec);

            if (product is null)
                    return NotFound(new ApiResponse(404));


            var result = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        {
          var brands=  await _brandRepo.GetAllAsync();
            return Ok(brands);
        }


        [Authorize]
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetCategories()
        {
            var Categories = await _categoryRepo.GetAllAsync();
            return Ok(Categories);
        }

    }
}
