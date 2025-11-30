using AuthKit.Cli.Interfaces;
using Spectre.Console.Cli.Help;

namespace AuthKit.Cli.Services;

/// <summary>
/// Groups CLI commands for help display by parent command or defaults to "Other".
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Excludes hidden commands from output.</item>
/// <item>Flattens subcommands under their parent for grouping.</item>
/// <item>Provides group descriptions from parent command's <see cref="ICommandInfo.Description"/>.</item>
/// </list>
/// </remarks>
internal class CommandGrouper : ICommandGrouper
{
    /// <inheritdoc />
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
    
    /// <inheritdoc />
    public string GetGroupDescription(string groupName, IEnumerable<ICommandInfo> commands)
    {
        var commandInfos = commands as ICommandInfo[] ?? commands.ToArray();
        var parent = commandInfos.FirstOrDefault(c => c.Name == groupName);
        if (parent != null && !string.IsNullOrWhiteSpace(parent.Description))
            return parent.Description!;
        return "";
    }
}