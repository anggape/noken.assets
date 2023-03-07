using CliFx.Attributes;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace Noken.Assets.Tools;

[Command("pack", Description = "Pack Assets")]
public sealed class PackCommand : Command
{
    [CommandOption("include", 'i', Description = "List of included files")]
    public string[]? Includes { get; set; }

    [CommandOption("exclude", 'e', Description = "List of excluded files")]
    public string[]? Excludes { get; set; }

    [CommandOption("output", 'o', Description = "Output file path")]
    public string? Output { get; set; }

    protected override ValueTask<int> Execute()
    {
        var includeMatcher = new Matcher();
        var excludeMatcher = new Matcher();

        (Includes is not null ? Includes : Config.Includes)
            .ToList()
            .ForEach(x => includeMatcher.AddInclude(x));
        (Excludes is not null ? Excludes : Config.Excludes)
            .ToList()
            .ForEach(x => excludeMatcher.AddInclude(x));

        var includes = includeMatcher
            .Execute(new DirectoryInfoWrapper(new(Directory.GetCurrentDirectory())))
            .Files.Select(x => x.Path);
        var excludes = excludeMatcher
            .Execute(new DirectoryInfoWrapper(new(Directory.GetCurrentDirectory())))
            .Files.Select(x => x.Path);
        var files = includes.Where(x => !excludes.Contains(x)).ToArray();
        var output = File.OpenWrite(Output is not null ? Output : Config.Output);

        var writer = new Writer(output, files);
        writer.Dispose();

        return new(0);
    }
}
