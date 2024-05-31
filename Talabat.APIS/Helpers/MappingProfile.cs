using AutoMapper;
using Talabat.APIS.Dtos;
using Talabat.Core.Entities;
using UserAddress = Talabat.Core.Entities.Identity.Address;
using orderAddress =Talabat.Core.Entities.Order.Address;
using Talabat.Core.Entities.Order;

namespace Talabat.APIS.Helpers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.Brand,o=>o.MapFrom(s=>s.Brand.Name))
                .ForMember(d=>d.Category,o=>o.MapFrom(s=>s.Category.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<UserAddress, AddressDto>().ReverseMap();

            CreateMap<orderAddress, AddressDto>().ReverseMap()
                .ForMember(D=>D.FirstName,D=>D.MapFrom(S=>S.Fname))
                .ForMember(D=>D.LastName,D=>D.MapFrom(S=>S.LName));


            CreateMap<Order, OrderToReturnDto>()
               .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
               .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));


             CreateMap<OrderItem, OrderItemDto>()
               .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
               .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
               .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

                CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();


        }
    }
}
