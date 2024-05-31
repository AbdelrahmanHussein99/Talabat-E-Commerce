using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.APIS.Middlewares;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;
using Talabat.APIS.Extebsions;
using StackExchange.Redis;
using Talabat.Repository.Identity;
using Microsoft.IdentityModel.Tokens;
using Talabat.Repository.Identity.DataSeed;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Microsoft.Extensions.Options;
using Talabat.Core.Services.Interfaces;
using Talabat.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace Talabat.APIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationbuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region configuration serivces
            webApplicationbuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationbuilder.Services.AddEndpointsApiExplorer();
            webApplicationbuilder.Services.AddSwaggerGen();



            webApplicationbuilder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webApplicationbuilder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationbuilder.Configuration.GetConnectionString("IdentityConnection"));
            });



            webApplicationbuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var configurations = webApplicationbuilder.Configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(configurations);
            });

            webApplicationbuilder.Services.AddApplicationServices();

            webApplicationbuilder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer= webApplicationbuilder.Configuration["JWT:ValidIssuer"],
                            ValidateAudience = true,
                            ValidAudience = webApplicationbuilder.Configuration["JWT:ValidAudience"],
                            ValidateLifetime = true,
                           IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationbuilder.Configuration["JWT:key"]))
                        };
 
                    });
            #endregion


            var app = webApplicationbuilder.Build();

           using var scope= app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _context = services.GetRequiredService<StoreDbContext>();

            var _IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();

           var loggerFactory= services.GetRequiredService<ILoggerFactory>();

            try
            {
               await  _context.Database.MigrateAsync();

               await  StoreDbContextSeed.SeedAsync(_context);

                await _IdentityDbContext.Database.MigrateAsync();


                var _UserManager = services.GetRequiredService<UserManager<AppUser>>();

               await AppIdentityDbContextSeed.SeedUsersAsync(_UserManager);

            }
            catch (Exception ex)
            {
              var logger =   loggerFactory.CreateLogger<Program>();

                logger.LogError(ex, "An error has been occured during Appling Migrations");
            }


            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
