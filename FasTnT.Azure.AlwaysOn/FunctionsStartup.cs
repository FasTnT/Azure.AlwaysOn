using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FasTnT.Azure.AlwaysOn.FunctionStartup))]
namespace FasTnT.Azure.AlwaysOn
{
    public class FunctionStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var url = Environment.GetEnvironmentVariable("EpcisRepository.Url");
            var username = Environment.GetEnvironmentVariable("EpcisRepository.Authorization.Username");
            var password = Environment.GetEnvironmentVariable("EpcisRepository.Authorization.Password");

            builder.Services
                .AddLogging()
                .AddSingleton(new EpcisRepositorySettings(url, username, password))
                .AddTransient<EpcisRepositoryService>();
        }
    }
}
