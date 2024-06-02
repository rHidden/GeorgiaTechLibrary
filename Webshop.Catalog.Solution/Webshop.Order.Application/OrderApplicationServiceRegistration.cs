using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Profiles;

namespace Webshop.Order.Application
{
    public static class OrderApplicationServiceRegistration
    {
        public static IServiceCollection AddOrderApplicationServices(this IServiceCollection services)
        {
            services.AddApplicationServices(); //register the general services from webshop.application
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));
            return services;
        }
    }
}
