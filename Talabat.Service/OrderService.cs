using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Interfaces;
using Talabat.Core.Specifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            var orderItems =new List<OrderItem>();   

            if (basket?.Items.Count()>0 ) 
            {
                foreach (var item in basket.Items)
                {
                    var product =await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered =new ProductItemOredered( product.Id, product.Name,product.PictureUrl );

                    var orderItem = new OrderItem(productItemOrdered, item.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            var subTotal =orderItems.Sum(OI=>OI.Price * OI.Quantity);

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            var spac = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);
           var ExOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpacAsync(spac);

            if(ExOrder is not null )
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
              basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }


            var order = new Order(BuyerEmail, ShippingAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);

            await _unitOfWork.Repository<Order>().AddAsync(order);//locally

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return order;
        }


        public async Task<IReadOnlyList<Order?>> GetOrdersForSpecificUerAsync(string BuyerEmail)
        {
            var spec = new OrderSpecifications(BuyerEmail);
           var orders = await _unitOfWork.Repository<Order>().GetAllWithSpacAsync(spec);

            return orders;
        }

        public async Task<Order?> GetOrdersByIdForSpecificUerAsync(string BuyerEmail, int OrderId)
        {

            var spec = new OrderSpecifications(BuyerEmail, OrderId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpacAsync(spec);
            if (order is null) return null;
            return order;

        }

    }
}
