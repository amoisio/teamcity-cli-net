using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("build")]
    public class BuildCommand : TeamCityItemCommand<IBuild>
    {
        public BuildCommand(TeamCity teamCity) : base(teamCity) { }

        protected override async Task<IBuild> Execute(IConsole console, TeamCity teamCity)
            => await teamCity.Builds.ById(Id).ConfigureAwait(false);
    }

    [Command("build latest")]
    public class BuildLatestCommand : ICommand
    {
        private readonly TeamCity _teamCity;

        public BuildLatestCommand(TeamCity teamCity) 
        { 
            _teamCity = teamCity;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var item = await _teamCity.Builds.Latest().ConfigureAwait(false);
            ItemPrinter.Print(console, item);
        }
    }

    [Command("build list")]
    public class BuildListCommand : TeamCityListCommand<IBuild>
    {
        public BuildListCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "BuildTypeId", "BuildNumber", "BranchName", "Status", "State" };

        [CommandOption("type", Description = "")]
        public string BuildType { get; set; }

        [CommandOption("snapsnot", Description = "")]
        public string Snapshot { get; set; }

        [CommandOption("andCanceled", Description = "")]
        public bool IncludeCanceled { get; set; }

        [CommandOption("andFailed", Description = "")]
        public bool IncludeFailed { get; set; }

        [CommandOption("andPersonal", Description = "")]
        public bool IncludePersonal { get; set; }

        [CommandOption("andRunning", Description = "")]
        public bool IncludeRunning { get; set; }

        [CommandOption("canceled", Description = "")]
        public bool OnlyCanceled { get; set; }

        [CommandOption("personal", Description = "")]
        public bool OnlyPersonal{ get; set; }

        [CommandOption("running", Description = "")]
        public bool OnlyRunning { get; set; }

        [CommandOption("pinned", Description = "")]
        public bool OnlyPinned { get; set; }

        [CommandOption("pageSize", Description = "")]
        public int? PageSize { get; set; }

        [CommandOption("since", Description = "")]
        public DateTime? Since{ get; set; }

        [CommandOption("until", Description = "")]
        public DateTime? Until { get; set; }

        [CommandOption("allBranches", Description = "")]
        public bool AllBranches { get; set; }
        
        [CommandOption("branch", Description  = "")]
        public string Branch { get; set; }

        [CommandOption("number", Description = "")]
        public string Number { get; set; }

        [CommandOption("tag", Description = "")]
        public string Tag { get; set; }

        [CommandOption("revision", Description = "")]
        public string Revision { get; set;}

        [CommandOption("status", Description = "")]
        public BuildStatus? Status { get; set; }
        protected override async Task<IEnumerable<IBuild>> Execute(IConsole console, TeamCity teamCity)
        {
            var locator = teamCity.Builds;
            if (!String.IsNullOrEmpty(BuildType)) locator.FromBuildType(new Id(BuildType));
            if (!String.IsNullOrEmpty(Snapshot)) locator.SnapshotDependencyTo(new Id(Snapshot));
            if (!String.IsNullOrEmpty(Branch)) locator.WithBranch(Branch);
            if (!String.IsNullOrEmpty(Number)) locator.WithNumber(Number);
            if (!String.IsNullOrEmpty(Tag)) locator.WithTag(Tag);
            if (!String.IsNullOrEmpty(Revision)) locator.WithVcsRevision(Revision);
            if (IncludeCanceled) locator.IncludeCanceled();
            if (IncludeFailed) locator.IncludeFailed();
            if (IncludePersonal) locator.IncludePersonal();
            if (IncludeRunning) locator.IncludeRunning();
            if (OnlyCanceled) locator.OnlyCanceled();
            if (OnlyPersonal) locator.OnlyPersonal();
            if (OnlyRunning) locator.OnlyRunning();
            if (OnlyPinned) locator.PinnedOnly();
            if (PageSize.HasValue) locator.PageSize(PageSize.Value);
            if (Since.HasValue) locator.Since(Since.Value);
            if (Until.HasValue) locator.Until(Until.Value);
            if (AllBranches) locator.WithAllBranches();
            if (Status.HasValue) locator.WithStatus(Status.Value);
            locator.LimitResults(Count);

            return await teamCity.Builds.All().ToArrayAsync().ConfigureAwait(false);        }
    }

    [Command("build fields")]
    public class BuildFieldsCommand : FieldsCommand<IBuild> { }
}