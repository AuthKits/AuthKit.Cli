using Spectre.Console.Cli.Help;

namespace AuthKit.Cli.Interfaces;

internal interface ICommandGrouper
{
    IEnumerable<IGrouping<string, ICommandInfo>> GroupCommands(IEnumerable<ICommandInfo> commands);

    string GetGroupDescription(string groupName, IEnumerable<ICommandInfo> commands);
}
