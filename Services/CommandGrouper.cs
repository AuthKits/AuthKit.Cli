using AuthKit.Cli.Interfaces;
using Spectre.Console.Cli.Help;

namespace AuthKit.Cli.Services;

internal class CommandGrouper : ICommandGrouper
{
    public IEnumerable<IGrouping<string, ICommandInfo>> GroupCommands(IEnumerable<ICommandInfo> commands)
    {
        var commandInfos = commands as ICommandInfo[] ?? commands.ToArray();

        return commandInfos
            .Where(c => !c.IsHidden)
            .SelectMany(c => c.Commands.Any() ? c.Commands : [c])
            .GroupBy(c =>
            {
                var parent = commandInfos.FirstOrDefault(b => b.Commands.Contains(c));
                return parent?.Name ?? "Other";
            })
            .OrderBy(g => g.Key);
    }

    public string GetGroupDescription(string groupName, IEnumerable<ICommandInfo> commands)
    {
        var commandInfos = commands as ICommandInfo[] ?? commands.ToArray();
        var parent = commandInfos.FirstOrDefault(c => c.Name == groupName);
        if (parent != null && !string.IsNullOrWhiteSpace(parent.Description))
            return parent.Description!;
        return "";
    }
}