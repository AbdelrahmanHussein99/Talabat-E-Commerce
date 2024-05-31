
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail,string basketId, int DeliveryMethodId,Address ShippingAddress);

        Task<IReadOnlyList<Order?>> GetOrdersForSpecificUerAsync(string BuyerEmail);

        Task<Order?> GetOrdersByIdForSpecificUerAsync(string BuyerEmail,int OrderId);
    }
}
