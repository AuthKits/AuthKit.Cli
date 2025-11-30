using Spectre.Console.Cli;

namespace AuthKit.Cli.Commands;

public class AddReferenceSettings : AddSettings
{
    [CommandArgument(0, "<PROJECT_REFERENCE>")]
    public string ProjectReference { get; set; }
}

public class AddReferenceCommand : AsyncCommand<AddReferenceSettings>
{
    public static void Configure(IConfigurator cfg)
    {
        cfg.AddCommand<AddPackageCommand>("reference")
            .WithDescription("test reference");
    }

    public override Task<int> ExecuteAsync(CommandContext context, AddReferenceSettings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}