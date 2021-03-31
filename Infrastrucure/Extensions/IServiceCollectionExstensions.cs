using Dashboard.Application.Repository;
using Dashboard.Application.Services;
using Dashboard.Infrastructure;
using Dashboard.Infrastructure.Repositories;
using Dashboard.Infrastrucure.Repositories;
using Dashboard.Infrastrucure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Infrastrucure.Extensions
{
    public static class IServiceCollectionExstensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDashboradService, DashboardService>();
            services.AddTransient(typeof(IRepository<>), typeof(MongoDbRepository<>));
            services.AddTransient<JobFactory>();
            services.AddTransient<DashbordRefreshJob>();

            return services;
        }
    }
}
