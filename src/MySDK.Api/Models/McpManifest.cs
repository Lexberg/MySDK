namespace MySDK.Api.Models;

public class McpManifest
{
    public string Version { get; set; } = "1.0.0";
    public ServerInfo Server { get; set; } = new();
    public List<Tool> Tools { get; set; } = new();
}

public class ServerInfo
{
    public string Name { get; set; } = "Vinmonopolet SDK";
    public string Description { get; set; } = "Tool for searching Norwegian wine catalog and getting wine recommendations";
    public string Version { get; set; } = "1.0.0";
}

public class Tool
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ToolInputSchema InputSchema { get; set; } = new();
}

public class ToolInputSchema
{
    public string Type { get; set; } = "object";
    public Dictionary<string, PropertySchema> Properties { get; set; } = new();
    public List<string> Required { get; set; } = new();
}

public class PropertySchema
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public object? Default { get; set; }
    public List<string>? Enum { get; set; }
}
