using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FasTnT.Azure.AlwaysOn.FunctionStartup))]
namespace FasTnT.Azure.AlwaysOn
{
    public class EpcisRepositorySettings
    {
        public string Url { get; }
        public string Username { get; }
        public string Password { get; }

        public EpcisRepositorySettings(string url, string username, string password)
        {
            Url = url;
            Username = username;
            Password = password;
        }
    }
}
