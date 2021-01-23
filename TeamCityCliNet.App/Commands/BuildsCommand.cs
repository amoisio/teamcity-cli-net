using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using TeamCityRestClientNet.Api;

namespace TeamCityCliNet.Commands
{
    [Command("builds")]
    public class BuildsCommand : TeamCityCommand
    {
        public BuildsCommand(TeamCity teamCity) : base(teamCity) { }

        public override string[] DefaultFields => new string[] {"Id", "BuildTypeId", "BuildNumber", "BranchName", "Status", "State" };

        [CommandOption("count", 'c', Description = "")]
        public override int? Count { get; set; } = 30;

        [CommandOption("latest", Description = "Gets the latest build.")]
        public bool Latest { get; set; }

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
        protected override async ValueTask Execute(IConsole console, TeamCity teamCity)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var item = await teamCity.Builds.ById(Id).ConfigureAwait(false);
                ItemPrinter.Print(console, item);
            }
            else if (Latest)
            {
                var item = await teamCity.Builds.Latest().ConfigureAwait(false);
                ItemPrinter.Print(console, item);
            }
            else 
            {
                var locator = teamCity.Builds;
                if (!String.IsNullOrEmpty(BuildType))   locator.FromBuildType(new Id(BuildType));
                if (!String.IsNullOrEmpty(Snapshot))    locator.SnapshotDependencyTo(new Id(Snapshot));
                if (!String.IsNullOrEmpty(Branch))      locator.WithBranch(Branch);
                if (!String.IsNullOrEmpty(Number))      locator.WithNumber(Number);
                if (!String.IsNullOrEmpty(Tag))         locator.WithTag(Tag);
                if (!String.IsNullOrEmpty(Revision))    locator.WithVcsRevision(Revision);
                if (IncludeCanceled)    locator.IncludeCanceled();
                if (IncludeFailed)      locator.IncludeFailed();
                if (IncludePersonal)    locator.IncludePersonal();
                if (IncludeRunning)     locator.IncludeRunning();
                if (OnlyCanceled)       locator.OnlyCanceled();
                if (OnlyPersonal)       locator.OnlyPersonal();
                if (OnlyRunning)        locator.OnlyRunning();
                if (OnlyPinned)         locator.PinnedOnly();
                if (PageSize.HasValue)  locator.PageSize(PageSize.Value);
                if (Since.HasValue)     locator.Since(Since.Value);
                if (Until.HasValue)     locator.Until(Until.Value);
                if (AllBranches)        locator.WithAllBranches();
                if (Status.HasValue)    locator.WithStatus(Status.Value);
                if (Count.HasValue)     locator.LimitResults(Count.Value);

                var items = await teamCity.Builds.All().ToArrayAsync().ConfigureAwait(false);
                TablePrinter.Print(console, items, Fields, Count);
            }
        }
    }

    [Command("builds fields")]
    public class BuildFieldsCommand : FieldsCommand<IBuild> { }
}