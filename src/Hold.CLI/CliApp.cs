using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Hold.CLI;

public class CliApp
{
    private readonly RootCommand _rootCommand;
    private readonly List<Option> _globalOptions = new();
    private readonly ServiceCollection _services = new();
    private readonly List<Func<InvocationContext, Task>> _middlewares = new();
    private IServiceProvider? _serviceProvider;

    public CliApp(string description = "")
    {
        _rootCommand = new RootCommand(description);
    }

    public void AddGlobalOption(Option option)
    {
        _rootCommand.AddGlobalOption(option);
        _globalOptions.Add(option);
    }

    public void AddMiddleware(Func<InvocationContext, Task> middleware)
    {
        _middlewares.Add(middleware);
    }

    public void ConfigureServices(Action<IServiceCollection> configure)
    {
        configure(_services);
    }

    public Command MapCommand(string name, Delegate action, string description = "")
    {
        return _rootCommand.MapCommand(name, action, description, _globalOptions, this);
    }

    public void MapDefault(Delegate action, string description = "")
    {
        _rootCommand.Description = description;
        CliAppExtensions.ConfigureCommand(_rootCommand, action, _globalOptions, this);
    }

    public async Task<int> RunAsync(string[] args)
    {
        _serviceProvider = _services.BuildServiceProvider();
        var builder = new CommandLineBuilder(_rootCommand);
        
        foreach (var middleware in _middlewares)
        {
            builder.AddMiddleware(async (context, next) => {
                context.BindingContext.AddService(typeof(IServiceProvider), _ => _serviceProvider);
                context.BindingContext.AddService(typeof(BankClient), _ => _serviceProvider!.GetRequiredService<BankClient>());
                await middleware(context);
                await next(context);
            });
        }

        builder.UseDefaults();
        return await builder.Build().InvokeAsync(args);
    }

    public IServiceProvider? GetServiceProvider() => _serviceProvider;
}

public static class CliAppExtensions
{
    public static Command MapCommand(this Command parent, string name, Delegate action, string description = "", List<Option>? globalOptions = null, CliApp? app = null)
    {
        var command = new Command(name, description);
        ConfigureCommand(command, action, globalOptions, app);
        parent.AddCommand(command);
        return command;
    }

    public static void ConfigureCommand(Command command, Delegate action, List<Option>? globalOptions = null, CliApp? app = null)
    {
        var method = action.Method;
        
        foreach (var parameter in method.GetParameters())
        {
            var optionName = $"--{KebabCase(parameter.Name ?? "")}";
            
            if (globalOptions != null && globalOptions.Any(o => o.HasAlias(optionName)))
            {
                continue;
            }

            var optionType = parameter.ParameterType;
            var genericOptionType = typeof(Option<>).MakeGenericType(optionType);
            var option = (Option)Activator.CreateInstance(genericOptionType, new object?[] { optionName, null })!;

            option.ArgumentHelpName = parameter.Name;
            command.AddOption(option);
        }

        command.Handler = CommandHandler.Create(action.Method, app?.GetServiceProvider());
    }

    private static string KebabCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
    }
}
