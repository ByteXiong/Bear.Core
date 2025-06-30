using System;

namespace BearPlatform.Common.MultiLanguage.Contract;

public class LocalizationOption
{
    public Type LocalizationType { get; set; }
    public string ResourcesPath { get; set; }
    public string DefaultCulture { get; set; }
    public string[] SupportedCultures { get; set; }
}
