namespace CodelyTv.Apps.Mooc.Backend.Extension.DependencyInjection
{
    using CodelyTv.Mooc.Courses.Domain;
    using CodelyTv.Mooc.Courses.Infrastructure.Persistence;
    using CodelyTv.Mooc.CoursesCounter.Domain;
    using CodelyTv.Mooc.CoursesCounter.Infrastructure.Persistence;
    using CodelyTv.Mooc.Shared.Infrastructure.Persistence.EntityFramework;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Shared.Domain;
    using Shared.Domain.Bus.Event;
    using Shared.Infrastructure;
    using Shared.Infrastructure.Bus.Event;
    using Shared.Infrastructure.Bus.Event.MsSql;
    using Shared.Infrastructure.Bus.Event.RabbitMq;

    public static class Infrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IRandomNumberGenerator, CSharpRandomNumberGenerator>();
            services.AddScoped<IUuidGenerator, CSharpUuidGenerator>();
            services.AddScoped<ICoursesCounterRepository, MsSqlCoursesCounterRepository>();
            services.AddScoped<ICourseRepository, MsSqlCourseRepository>();

            services.AddScoped<IEventBus, RabbitMqEventBus>();
            services.AddScoped<IEventBusConfiguration, RabbitMqEventBusConfiguration>();
            services.AddScoped<InMemoryApplicationEventBus, InMemoryApplicationEventBus>();
            
            // Failover
            services.AddScoped<MsSqlEventBus, MsSqlEventBus>();
            
            services.AddScoped<RabbitMqDomainEventsConsumer, RabbitMqDomainEventsConsumer>();
            services.AddScoped<DomainEventsInformation, DomainEventsInformation>();

            services.AddScoped<DbContext, MoocContext>();
            services.AddDbContext<MoocContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MoocDatabase")), ServiceLifetime.Transient);

            services.AddRabbitMq(configuration);

            services.AddScoped<DomainEventJsonDeserializer, DomainEventJsonDeserializer>();

            return services;
        }

        private static IServiceCollection AddRabbitMq(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<RabbitMqPublisher, RabbitMqPublisher>();
            services.AddScoped<RabbitMqConfig, RabbitMqConfig>();
            services.Configure<RabbitMqConfigParams>(configuration.GetSection("RabbitMq"));

            return services;
        }
    }
}