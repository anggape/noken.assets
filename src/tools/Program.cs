using CliFx;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

// csharpier-ignore
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "noken.log"))
    .CreateLogger();

await new CliApplicationBuilder()
    .SetTitle("Noken Assets")
    .SetDescription("Noken Command Line Tools")
    .SetExecutableName("noken-assets")
    .AddCommandsFromThisAssembly()
    .Build()
    .RunAsync(args);
