using Spectre.Console.Cli;

namespace AuthKit.Cli.Commands;

public class AddSettings : CommandSettings
{
    [CommandArgument(0, "[PROJECT]")]
    public string Project { get; set; }
}

public class AddPackageSettings : AddSettings
{
    [CommandArgument(0, "<PACKAGE_NAME>")]
    public string PackageName { get; set; }

    [CommandOption("-v|--version <VERSION>")]
    public string Version { get; set; }
}


public class AddPackageCommand : AsyncCommand<AddPackageSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, AddPackageSettings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    public static void Configure(IConfigurator cfg)
    {
        cfg.AddCommand<AddPackageCommand>("package")
            .WithDescription("test package");
    }
}
