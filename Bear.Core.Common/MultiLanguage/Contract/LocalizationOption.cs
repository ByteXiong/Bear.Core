using System;

namespace Bear.Core.Common.MultiLanguage.Contract;

public class LocalizationOption
{
    public Type LocalizationType { get; set; }
    public string ResourcesPath { get; set; }
    public string DefaultCulture { get; set; }
    public string[] SupportedCultures { get; set; }
}
