using Application.Repository.Interface;
using Domain.Models.Entites;
using Infrastructure.Data;
using Infrastructure.Repository.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
            });

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Login";
            //    options.AccessDeniedPath = "/AccessDenied";
            //});

            //services.AddIdentity<User, IdentityRole<int>>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddScoped(typeof(IBasicRepository<>), typeof(BasicRepository<>));
            services.AddScoped<ICurrencyRatesRepository, CurrencyRatesRepository>();
            services.AddScoped<ICurrencyExchangeTransactionRepository, CurrencyExchangeTransactionRepository>();
            services.AddScoped<ICurrencyTypesRepository, CurrencyTypesRepository>();

            return services;
        }
    }
}
