using Application.Common.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .LogTo(Console.WriteLine, LogLevel.Information));
                

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IUnitOfWork, UnitOfWork>(x =>
            {
                return new UnitOfWork(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IDbTransaction>((serviceProvider) =>
            {
                var connection = serviceProvider
                    .GetService<IDbConnection>();
                connection.Open();

                return connection.BeginTransaction();
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IEmailService, EmailService>();
            services.Configure<AuthMessageSenderOptions>(configuration);

            return services;
        }
    }
}
