using System;
using Serilog;

namespace Bear.Core.Common.Helper.Serilog;

public class SerilogManager
{
    public static ILogger GetLogger(Type type)
    {
        return Log.ForContext("SourceContext", type.FullName);
    }
}
