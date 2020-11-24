﻿using Css.Api.AdminOps.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Css.Api.AdminOps.Extensions
{
    /// <summary>
    /// Extension for adding the DB Context
    /// </summary>
    public static class DBContextExtension
    {
        /// <summary>
        /// Adds the database context configuration.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddDBContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AdminOpsContext>(options =>
                 options.UseMySQL(configuration.GetConnectionString("Database")));

            return services;
        }
    }
}
