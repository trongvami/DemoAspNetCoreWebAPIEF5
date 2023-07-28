namespace DemoFullIdentityEF6.Common
{
    public static class Utilities
    {
        private static IConfigurationRoot Configuration { get; }
        static Utilities()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // requires Microsoft.Extensions.Configuration.Json
                .AddJsonFile("appsettings.json") // requires Microsoft.Extensions.Configuration.Json
                .AddEnvironmentVariables(); // requires Microsoft.Extensions.Configuration.EnvironmentVariables
            Configuration = builder.Build();
        }

        public static IConfigurationSection GetSection(string keyName)
        {
            return Configuration.GetSection(keyName);
        }
    }
}
