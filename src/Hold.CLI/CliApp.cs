using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Reflection;

namespace Hold.CLI;

public class CliApp
{
    private readonly RootCommand _rootCommand;

    public CliApp(string description = "")
    {
        _rootCommand = new RootCommand(description);
    }

    public Command MapCommand(string name, Delegate action, string description = "")
    {
        return _rootCommand.MapCommand(name, action, description);
    }

    public void MapDefault(Delegate action, string description = "")
    {
        _rootCommand.Description = description;
        CliAppExtensions.ConfigureCommand(_rootCommand, action);
    }

    public async Task<int> RunAsync(string[] args)
    {
        return await _rootCommand.InvokeAsync(args);
    }
}

public static class CliAppExtensions
{
    public static Command MapCommand(this Command parent, string name, Delegate action, string description = "")
    {
        var command = new Command(name, description);
        ConfigureCommand(command, action);
        parent.AddCommand(command);
        return command;
    }

    public static void ConfigureCommand(Command command, Delegate action)
    {
        var method = action.Method;
        foreach (var parameter in method.GetParameters())
        {
            var optionName = $"--{KebabCase(parameter.Name ?? "")}";
            var optionType = parameter.ParameterType;

            var genericOptionType = typeof(Option<>).MakeGenericType(optionType);
            var option = (Option)Activator.CreateInstance(genericOptionType, new object?[] { optionName, null })!;

            option.ArgumentHelpName = parameter.Name;
            command.AddOption(option);
        }

        command.Handler = CommandHandler.Create(action);
    }

    private static string KebabCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
    }
}
