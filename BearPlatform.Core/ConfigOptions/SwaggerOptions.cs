using BearPlatform.Common.Attributes;

namespace BearPlatform.Core.ConfigOptions;

[OptionsSettings]
public class SwaggerOptions
{
    public bool Enabled { get; set; }

    public string Title { get; set; }

    public string Route { get; set; }
    public List<SwaggerDesc> Desc { get; set; }
}

public class SwaggerDesc
{
    public string Name { get; set; }
    public string Version { get; set; }
    public bool IsDeprecated { get; set; }
}
