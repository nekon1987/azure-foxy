using System;
using System.Linq;
using ImageProcessor;
using ImageProcessor.Core;
using ImageProcessor.Core.SystemConfiguration;
using ImageProcessor.Core.SystemConfiguration.Enums;
using ImageProcessor.Core.SystemConfiguration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: WebJobsStartup(typeof(Startup))]
namespace ImageProcessor
{
    public class Startup : IWebJobsStartup
    {
        private static readonly StartupService StartupService = new StartupService();

        public void Configure(IWebJobsBuilder builder)
        {
            var configurationBuilder = StartupService.GetCurrentConfigurationObject(builder);
            var currentConfiguration = configurationBuilder.Build();
            var currentRunningEnvironment = StartupService.CheckCurrentApplicationEnvironmentCategory(currentConfiguration);

            switch (currentRunningEnvironment)
            {
                case EnvironmentCategory.LocalDevelopment:
                    StartupService.ConfigureLocalDevelopmentSecretsStorage(configurationBuilder);
                    break;
                case EnvironmentCategory.Production:
                    StartupService.ConfigureAzureKeyVaultSecretsStorage(configurationBuilder, currentConfiguration);
                    break;
            }
            

            currentConfiguration = configurationBuilder.Build();
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), currentConfiguration));

            ConfigurationManager.Initialize(currentConfiguration);
        }
    }
}
