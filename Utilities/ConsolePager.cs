using Spectre.Console;
using Spectre.Console.Rendering;

namespace AuthKit.Cli.Utilities;

/// <summary>
/// Utility for displaying collections of <see cref="IRenderable"/> items in a paginated console view.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Supports interactive paging with keyboard controls (Next, Previous, Quit).</item>
/// <item>Automatically adapts to redirected input/output by rendering all items sequentially.</item>
/// <item>Uses Spectre.Console for rich console rendering.</item>
/// </list>
/// </remarks>
internal static class ConsolePager
{
    /// <summary>
    /// Displays a sequence of <see cref="IRenderable"/> items with optional pagination.
    /// </summary>
    /// <param name="items">The items to display.</param>
    /// <param name="pageSize">The number of items per page. Default is 15.</param>
    public static void Show(IEnumerable<IRenderable> items, int pageSize = 15)
    {
        var list = items.ToList();
        if (!Console.IsInputRedirected && !Console.IsOutputRedirected && list.Count > pageSize)
        {
            var page = 0;
            var totalPages = (int)Math.Ceiling(list.Count / (double)pageSize);

            while (true)
            {
                AnsiConsole.WriteLine();

                foreach (var item in list.Skip(page * pageSize).Take(pageSize))
                    AnsiConsole.Write(item);

                AnsiConsole.MarkupLine($"\n[grey]Page {page + 1}/{totalPages} — N: next | P: prev | Q: quit[/]");

                var key = Console.ReadKey(true).Key;
                page = key switch
                {
                    ConsoleKey.N when page < totalPages - 1 => page + 1,
                    ConsoleKey.P when page > 0 => page - 1,
                    ConsoleKey.Q => -1,
                    _ => page
                };

                if (page == -1)
                    break;
            }
            
            return;
        }

        foreach (var item in list)
            AnsiConsole.Write(item);
    }
}