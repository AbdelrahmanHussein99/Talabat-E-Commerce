using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Interfaces;
using Talabat.Repository;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;
using Talabat.Service;

namespace Talabat.APIS.Extebsions
{
    public static class ApplicationServicesEntensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddAutoMapper(typeof(MappingProfile));



            services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                .SelectMany(p => p.Value.Errors)
                                                 .Select(E => E.ErrorMessage).ToArray();
                    var validationErrorRespone = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorRespone);
                };
            });
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));


            services.AddIdentity<AppUser, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
