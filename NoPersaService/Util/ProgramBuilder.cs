using FluentValidation;
using NoPersaService.FluentValidations;
using NoPersaService.MappingProfiles;
using NoPersaService.Services;
using System.Reflection;

namespace NoPersaService.Util
{
    public static class ProgramBuilder
    {
        public static void RegisterHostedServices(IServiceCollection services)
        {
            services.AddHostedService<ArticleService>();
            services.AddHostedService<CustomersBoxStatusService>();
            services.AddHostedService<DailyDeliveryService>();
        }

        public static void RegisterAutoMapperProfiles(IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(ManagementProfile));
            var profiles = assembly?.GetTypes()
                .Where(t => "NoPersaService.MappingProfiles".Equals(t.Namespace)).ToArray();

            if (profiles != null && profiles.Length != 0) 
            {
                services.AddAutoMapper(profiles);
            }
        }

        public static void RegisterFluentValidations(IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(CustomerValidator));
            var validators = assembly?.GetTypes()
                .Where(t => "NoPersaService.FluentValidations".Equals(t.Namespace)).ToList();

            foreach (var validator in validators ?? [])
            {
                var genericArg = validator.BaseType!.GetGenericArguments().FirstOrDefault();
                if (genericArg != null)
                {
                    var serviceType = typeof(IValidator<>).MakeGenericType(genericArg);
                    services.AddScoped(serviceType, validator);
                }
            }
        }
    }
}
