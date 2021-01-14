using System;
using TeamCityRestClientNet;
using CliFx;
using System.Threading.Tasks;
// TeamCityRestClientNet, TeamCityRestClientNet.Api, TeamCityRestClientNet.Domain, TeamCityRestClientNet.Tools, TeamCityRestClientNet.Extensions

namespace TeamCityCliNet.App
{
    public static class Program
    {
        // private static readonly string _serverUrl = "http://localhost:5000";
        // private static readonly string _token = "eyJ0eXAiOiAiVENWMiJ9.Tkp4RUN4RGpWbl8wNy1KVG5EbmxsZXpWaDIw.ZTRmYTc3NDUtYTQ3OS00ZmMzLWJkMTAtMTU0OTE1YWVlOGI4";

        public async static Task<int> Main(string[] args)
            => await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync();
    }
}
