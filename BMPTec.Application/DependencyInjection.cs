using BMPTec.Application.Common.Behaviors;
using FluentValidation; // Explicitly import AutoMapper to use its extension method unambiguously
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BMPTec.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // MediatR
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
            });

            // Pipeline Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // AutoMapper
            services.AddAutoMapper(assembly);

            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
