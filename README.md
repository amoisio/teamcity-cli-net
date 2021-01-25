Root

tc

Entities

Agent
AgentPool
Build
BuildQueue
BuildType
Changes
Investigations
ProblemOccurrences
Project
TestOccurrences
User
VcsRoot

Common options

-h, --help  display command help

Common commands

list - lists all entities. For some entities can provide an optional locator to limit the result set. Examples tc [entity] list [locator].

-> if locator is not a locator string, then call byid

Commands cases

tc run <builtTypeId> [-b|--branch] 


tc user list







TODO:

1. I typically query a entity, like projects
2. look up the interesting bit in the list
3. Then want to view that entity more closely

- Currently this requires
1. ./tc projects
2. copy id
3. ./tc projects --id <paste id>

I wonder if I could make commands history aware so I could do something like this
1. ./tc projects | ./tc project --id --

The second line would look up the entity (projects in this case), and the id of the first rows, and then query 