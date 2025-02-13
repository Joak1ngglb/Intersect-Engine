﻿using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Intersect.Config;

public partial class DatabaseOptions
{
#if DEBUG
    private const bool DefaultKillServerOnConcurrencyException = true;
#else
    private const bool DefaultKillServerOnConcurrencyException = false;
#endif

    [JsonConverter(typeof(StringEnumConverter))]
    public DatabaseType Type { get; set; } = DatabaseType.SQLite;

    public string Server { get; set; } = "localhost";

    public ushort Port { get; set; } = 3306;

    public string Database { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty(
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Populate
    )]
    [DefaultValue(Microsoft.Extensions.Logging.LogLevel.Error)]
    public Microsoft.Extensions.Logging.LogLevel LogLevel { get; set; } = Microsoft.Extensions.Logging.LogLevel.Error;

    public bool KillServerOnConcurrencyException { get; set; } = DefaultKillServerOnConcurrencyException;
}
