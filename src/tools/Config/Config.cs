using Newtonsoft.Json;
using Serilog;

namespace Noken.Assets.Tools;

public sealed class Config
{
    private const string ConfigFile = "noken.json";

    [JsonProperty("includes")]
    public string[] Includes { get; set; } = new string[0];

    [JsonProperty("excludes")]
    public string[] Excludes { get; set; } = new string[0];

    [JsonProperty("output")]
    public string Output { get; set; } = "resources.noken";

    public static Config Read()
    {
        if (!File.Exists(ConfigFile))
            return new();

        var result = JsonConvert.DeserializeAnonymousType(
            File.ReadAllText(ConfigFile),
            new { assets = new Config() }
        );

        if (result is null)
        {
            Log.Error("Failed to parse config file");
            Environment.Exit(1);
        }

        return result.assets;
    }
}
