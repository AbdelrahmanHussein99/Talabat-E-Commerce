using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications
{
    public class OrderWithPaymentIntentSpecifications :BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecifications(string PaymentIntentId):base(O=>O.PaymentIntenId==PaymentIntentId)
        {
            
        }
    }
}
