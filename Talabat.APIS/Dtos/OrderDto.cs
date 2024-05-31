using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        [Required]

        public int DeliveryMethodId { get; set; }
        [Required]

        public AddressDto ShippingAddress { get; set; }
    }
}
