using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Services.Interfaces
{
    public interface IPaymentService
    {
     Task<CustomerBasket>  CreateOrUpdatePaymentIntent(string basketId);


       Task<Order> UpdatePaymentIntentToSuccessedOrFailed(string PaymentIntentId, bool flag);
    }
}
