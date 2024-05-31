using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.APIS.Dtos;
using Talabat.APIS.Errors;
using Talabat.Core;
using Order = Talabat.Core.Entities.Order.Order;
using Talabat.Core.Services.Interfaces;
using Talabat.Core.Entities.Order;

namespace Talabat.APIS.Controllers
{

    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto model)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var address = _mapper.Map<AddressDto, Address>(model.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, model.BasketId, model.DeliveryMethodId, address);

            if (order is null) return BadRequest(new ApiResponse(400, "There is a problem with your order"));


           var result = _mapper.Map<Order, OrderToReturnDto>(order);
            return Ok(result);

        }

        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);


            var orders = await _orderService.GetOrdersForSpecificUerAsync(buyerEmail);

            if (orders is null) return NotFound(new ApiResponse(404, "There is no order for U"));

            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));

        }


        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);


            var order = await _orderService.GetOrdersByIdForSpecificUerAsync(buyerEmail, id);

            if (order is null) return NotFound(new ApiResponse(404, $"There is no order with id {id} for U"));

            return Ok(_mapper.Map<OrderToReturnDto>(order));

        }
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
           var DeliveryMethods= await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return Ok(DeliveryMethods);
        }
    }
}
