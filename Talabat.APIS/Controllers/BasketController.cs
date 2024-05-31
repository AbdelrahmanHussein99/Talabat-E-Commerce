using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;
using Talabat.APIS.Errors;
using Talabat.APIS.Dtos;
using AutoMapper;

namespace Talabat.APIS.Controllers
{
   
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<CustomerBasket>> GetBasket(string id) 
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok(basket?? new CustomerBasket(id));

        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket) 
        {
          var mappedBasket = _mapper.Map<CustomerBasket>(basket);

            var CreatedOrUpdatebasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (CreatedOrUpdatebasket is null) return BadRequest(new ApiResponse(400));

            return Ok(CreatedOrUpdatebasket);
        }

        [HttpDelete]

        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }

    }
}
