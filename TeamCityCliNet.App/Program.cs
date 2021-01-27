using System;
using TeamCityRestClientNet;
using CliFx;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using Microsoft.Extensions.DependencyInjection;
using TeamCityCliNet.Commands;
// TeamCityRestClientNet, TeamCityRestClientNet.Api, TeamCityRestClientNet.Domain, TeamCityRestClientNet.Tools, TeamCityRestClientNet.Extensions

namespace TeamCityCliNet.App
{
    public static class Program
    {
        private static readonly string _serverUrl = "http://localhost:5000";
        private static readonly string _token = "eyJ0eXAiOiAiVENWMiJ9.Tkp4RUN4RGpWbl8wNy1KVG5EbmxsZXpWaDIw.ZTRmYTc3NDUtYTQ3OS00ZmMzLWJkMTAtMTU0OTE1YWVlOGI4";
        public async static Task<int> Main(string[] args)
        {
            var services = new ServiceCollection();

            // Services
            services.AddScoped<TeamCity>(provider => BuildTeamCity());

            // Commands
            services.AddTransient<AgentShowCommand>();
            services.AddTransient<AgentEnableCommand>();
            services.AddTransient<AgentDisableCommand>();
            services.AddTransient<AgentListCommand>();
            services.AddTransient<AgentFieldsCommand>();

            services.AddTransient<AgentPoolCommand>();
            services.AddTransient<AgentPoolListCommand>();
            services.AddTransient<AgentPoolFieldsCommand>();

            services.AddTransient<BuildCommand>();
            services.AddTransient<BuildLatestCommand>();
            services.AddTransient<BuildListCommand>();
            services.AddTransient<BuildFieldsCommand>();

            services.AddTransient<BuildQueueCommand>();
            services.AddTransient<BuildQueueFieldsCommand>();

            services.AddTransient<BuildConfigurationCommand>();
            services.AddTransient<BuildConfigurationListCommand>();
            services.AddTransient<BuildConfigurationFieldsCommand>();

            services.AddTransient<ChangeCommand>();
            services.AddTransient<ChangeTypeCommand>();
            services.AddTransient<ChangeListCommand>();
            services.AddTransient<ChangeFieldsCommand>();
            
            services.AddTransient<ProjectsCommand>();
            services.AddTransient<ProjectDeleteCommand>();
            services.AddTransient<ProjectFieldsCommand>();
            services.AddTransient<RunCommand>();
            services.AddTransient<UsersCommand>();
            services.AddTransient<UserFieldsCommand>();
            services.AddTransient<VcsRootsCommand>();
            services.AddTransient<VcsRootFieldsCommand>();

            var serviceProvider = services.BuildServiceProvider();
            var returnCode = await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(serviceProvider.GetService)
                .Build()
                .RunAsync();

            return returnCode;
        }

        private static TeamCity BuildTeamCity()
            => new TeamCityServerBuilder()
                .WithServerUrl(_serverUrl)
                .WithBearerAuthentication(_token)
                .Build();
    }
}
