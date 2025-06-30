using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BearPlatform.Common.Global;

/// <summary>
/// 程序集类型
/// </summary>
public static class GlobalType
{
    public const string ApiAssembly = "BearPlatform.Api";
    public const string CoreAssembly = "BearPlatform.Core";
    public const string CommonAssembly = "BearPlatform.Common";
    public const string IBusinessAssembly = "BearPlatform.IBusiness";
    public const string BusinessAssembly = "BearPlatform.Business";
    public const string RepositoryAssembly = "BearPlatform.Repository";
    public const string TaskServiceAssembly = "BearPlatform.TaskService";
    public const string EntityAssembly = "BearPlatform.Entity";
    public const string SharedModelAssembly = "BearPlatform.Models";
    public const string ViewModelAssembly = "BearPlatform.ViewModel";
    public const string EventBusAssembly = "BearPlatform.EventBus";

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
