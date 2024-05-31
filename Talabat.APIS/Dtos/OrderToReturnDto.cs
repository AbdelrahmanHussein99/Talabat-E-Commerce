using Talabat.Core.Entities.Order;

namespace Talabat.APIS.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public string Status { get; set; }

        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal SubTotal { get; set; }
      
        public decimal Total { get; set; }

        public string PaymentIntenId { get; set; } = string.Empty;
    }
}
