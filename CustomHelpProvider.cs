using AuthKit.Cli.Interfaces;
using AuthKit.Cli.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;

namespace AuthKit.Cli;

internal class CustomHelpProvider(ICommandAppSettings settings, ICommandGrouper grouper) : HelpProvider(settings)
{
    public override IEnumerable<IRenderable> GetHeader(ICommandModel model, ICommandInfo? command)
    {
        return
        [
            new Text("--------------------------------------"), Text.NewLine,
            new Text("---      CUSTOM HELP PROVIDER      ---"), Text.NewLine,
            new Text("--------------------------------------"), Text.NewLine,
            Text.NewLine
        ];
    }
    
    /*
    public override IEnumerable<IRenderable> GetUsage(ICommandModel model, ICommandInfo? command)
    {
        yield return new Text("USAGE: ./authkit <command> [options]");
    }
    */

    public override IEnumerable<IRenderable> GetCommands(ICommandModel model, ICommandInfo? command)
    {
        var container = command ?? (ICommandContainer)model;
        var commands = container.Commands.Where(c => !c.IsHidden).ToList();
        if (!commands.Any()) yield break;

        foreach (var group in grouper.GroupCommands(commands))
        {
            var groupDescription = grouper.GetGroupDescription(group.Key, commands) ?? "";

            yield return new Markup($"[green]{group.Key} commands:[/]");
            if (!string.IsNullOrWhiteSpace(groupDescription))
                yield return new Text($"    {groupDescription}");
            yield return Text.NewLine;

            var grid = new Grid();
            grid.AddColumn(new GridColumn { Padding = new Padding(2, 2), NoWrap = true });
            grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

            foreach (var child in group.OrderBy(c => c.Name))
            {
                var commandText = child.Parameters
                    .OfType<ICommandArgument>()
                    .Where(a => a.IsRequired)
                    .Aggregate(child.Name, (current, arg) => current + $" <{arg.Value}>");

                var description = string.IsNullOrWhiteSpace(child.Description) ? "" : child.Description;

                grid.AddRow(
                    new Markup($"[yellow]{commandText}[/]"),
                    new Text(description)
                );
            }

            yield return grid;
            yield return Text.NewLine;
        }
    }


}