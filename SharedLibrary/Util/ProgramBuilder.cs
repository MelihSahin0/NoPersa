using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.FluentValidations;
using System.Reflection;

namespace SharedLibrary.Util
{
    public static class ProgramBuilder
    {
        public static List<(Type, Type)> GetFluentValidations()
        {
            List<(Type, Type)> result = [];

            var assembly = Assembly.GetAssembly(typeof(CustomerValidator));
            var validators = assembly?.GetTypes()
                .Where(t => t.Namespace == "SharedLibrary.FluentValidations").ToList();

            foreach (var validator in validators ?? [])
            {
                var genericArg = validator.BaseType!.GetGenericArguments().FirstOrDefault();
                if (genericArg != null)
                {
                    var serviceType = typeof(IValidator<>).MakeGenericType(genericArg);
                    result.Add((serviceType, validator));
                }
            }

            return result;
        }

        public static IServiceProvider? serviceProvider = null;
        public static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            foreach (var validator in ProgramBuilder.GetFluentValidations())
            {
                serviceCollection.AddScoped(validator.Item1, validator.Item2);
            }

            if (serviceProvider != null)
            {
                return serviceProvider;
            }
            else
            {
                serviceProvider = serviceCollection.BuildServiceProvider(); 
                return serviceProvider;
            }
        }
    }
}
