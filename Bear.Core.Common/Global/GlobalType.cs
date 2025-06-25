using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bear.Core.Common.Global;

/// <summary>
/// 程序集类型
/// </summary>
public static class GlobalType
{
    public const string ApiAssembly = "Bear.Core.Api";
    public const string CoreAssembly = "Bear.Core.Core";
    public const string CommonAssembly = "Bear.Core.Common";
    public const string IBusinessAssembly = "Bear.Core.IBusiness";
    public const string BusinessAssembly = "Bear.Core.Business";
    public const string RepositoryAssembly = "Bear.Core.Repository";
    public const string TaskServiceAssembly = "Bear.Core.TaskService";
    public const string EntityAssembly = "Bear.Core.Entity";
    public const string SharedModelAssembly = "Bear.Core.Models";
    public const string ViewModelAssembly = "Bear.Core.ViewModel";
    public const string EventBusAssembly = "Bear.Core.EventBus";

    public static readonly List<Type> ApiTypes;
    public static readonly List<Type> CoreTypes;
    public static readonly List<Type> CommonTypes;
    public static readonly List<Type> IBusinessTypes;
    public static readonly List<Type> BusinessTypes;
    public static readonly List<Type> RepositoryTypes;
    public static readonly List<Type> TaskServiceTypes;
    public static readonly List<Type> EntityTypes;
    public static readonly List<Type> SharedModelTypes;
    public static readonly List<Type> ViewModelTypes;
    public static readonly List<Type> EventBusTypes;

    static GlobalType()
    {
        ApiTypes = LoadAssemblyTypes(ApiAssembly);
        CoreTypes = LoadAssemblyTypes(CoreAssembly);
        CommonTypes = LoadAssemblyTypes(CommonAssembly);
        IBusinessTypes = LoadAssemblyTypes(IBusinessAssembly);
        BusinessTypes = LoadAssemblyTypes(BusinessAssembly);
        RepositoryTypes = LoadAssemblyTypes(RepositoryAssembly);
        TaskServiceTypes = LoadAssemblyTypes(TaskServiceAssembly);
        EntityTypes = LoadAssemblyTypes(EntityAssembly);
        SharedModelTypes = LoadAssemblyTypes(SharedModelAssembly);
        ViewModelTypes = LoadAssemblyTypes(ViewModelAssembly);
        EventBusTypes = LoadAssemblyTypes(EventBusAssembly);
    }

    private static List<Type> LoadAssemblyTypes(string dllName)
    {
        var assembly = LoadAssembly(dllName + ".dll");
        return assembly.GetTypes().Where(u => u.IsPublic).ToList();
    }

    private static Assembly LoadAssembly(string dllName)
    {
        var basePath = AppContext.BaseDirectory;
        var dllFile = Path.Combine(basePath, dllName);
        if (!File.Exists(dllFile))
        {
            throw new System.Exception($"{dllName} 文件未生成, 编译项目成功后重试！");
        }

        return Assembly.LoadFrom(dllFile);
    }
}
