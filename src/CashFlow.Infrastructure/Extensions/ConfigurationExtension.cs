using Microsoft.Extensions.Configuration;

namespace CashFlow.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static bool IsTestEnvironment(this IConfiguration config)
        {
            return config.GetValue<bool>("InMemoryTest");
        }
    }
}
