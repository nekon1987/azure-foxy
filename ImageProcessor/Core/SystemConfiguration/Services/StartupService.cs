using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageProcessor.Core.SystemConfiguration.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace ImageProcessor.Core.SystemConfiguration.Services
{
    public class StartupService
    {
        public EnvironmentCategory CheckCurrentApplicationEnvironmentCategory(IConfigurationRoot config)
        {
            var environmentCategoryLetter = config["EnvironmentCategory"].ToUpper();
            switch (environmentCategoryLetter)
            {
                case "P": return EnvironmentCategory.Production;
                case "D": return EnvironmentCategory.LocalDevelopment;
            }
            throw new Exception("Invalid or missing EnvironmentCategory setting.");
        }

        public ConfigurationBuilder GetCurrentConfigurationObject(IWebJobsBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (descriptor?.ImplementationInstance is IConfigurationRoot configuration)
            {
                configurationBuilder.AddConfiguration(configuration);
            }
            return configurationBuilder;
        }

        public void ConfigureLocalDevelopmentSecretsStorage(ConfigurationBuilder builder)
        {
            builder.AddUserSecrets<Startup>();
        }

        public void ConfigureAzureKeyVaultSecretsStorage(ConfigurationBuilder builder, IConfigurationRoot config)
        {
            var vaultUrl = $"https://kvlt-neu-{config["EnvironmentCategory"]}-foxy-01.vault.azure.net/";
            var vaultClientId = config["VaultClientId"];
            var vaultClientSecret = config["VaultClientSecret"];
            builder.AddAzureKeyVault(vaultUrl, vaultClientId, vaultClientSecret);

            builder.AddUserSecrets<Startup>();
        }
    }
}
