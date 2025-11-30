using AuthKit.Cli.Commands;
using AuthKit.Cli.Services;
using AuthKit.Cli.Spectre;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace AuthKit.Cli;

public sealed class ConsoleApp(IServiceCollection services)
{

    public void Run(string[] args)
    {
        Console.WriteLine("?? Starting AuthKit CLI...");
        
        var registrar = new TypeRegistrar(services);
        var app = new CommandApp(registrar);
        var grouper = new CommandGrouper();
        
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.SetHelpProvider(new CustomHelpProvider(config.Settings, grouper));
            TestCommand.Configure(config);
            
            config.AddBranch<AddSettings>("add", add =>
            {
                add.SetDescription("Commands for adding packages or references");
                add.AddCommand<AddPackageCommand>("package")
                    .WithDescription("Add a package to the project");
                add.AddCommand<AddReferenceCommand>("reference")
                    .WithDescription("Add a reference to the project");
            });
        });
       
        app.Run(args);
    }
}