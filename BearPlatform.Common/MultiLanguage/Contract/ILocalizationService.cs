using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace BearPlatform.Common.MultiLanguage.Contract;

/// <summary>
/// 语言本地化服务的接口。
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// 获取带有给定名称的字符串资源。
    /// </summary>
    /// <param name="name">字符串资源的名称。</param>
    string R(string name);

    /// <summary>
    /// 获取带有给定名称的字符串资源，并用提供的参数格式化。
    /// </summary>
    /// <param name="name">字符串资源的名称。</param>
    /// <param name="arguments">使用字符串格式化的值。</param>
    string R(string name, params object[] arguments);


    /// <summary>
    /// 获取所有字符串资源。
    /// <param name="includeParentCultures">指示是否包括父母文化的字符串。</param>
    /// </summary>
    IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures);
}
