using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Bear.Core.Common.ClassLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bear.Core.Common.Extensions;

/// <summary>
/// 字符串扩展
/// </summary>
public static partial class ExtObject
{
    /// <summary>
    /// 转为字节数组
    /// </summary>
    /// <param name="base64Str">base64字符串</param>
    /// <returns></returns>
    public static byte[] ToBytes_FromBase64Str(this string base64Str)
    {
        return Convert.FromBase64String(base64Str);
    }

    /// <summary>
    /// 转换为MD5加密后的字符串（默认加密为32位）
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToMd5String(this string str)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(str);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("x2"));
        }

        md5.Dispose();

        return sb.ToString();
    }

    /// <summary>
    /// 转换为MD5加密后的字符串（16位）
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToMd5String16(this string str)
    {
        return str.ToMd5String().Substring(8, 16);
    }

    /// <summary>
    /// Base64加密
    /// 注:默认采用UTF8编码
    /// </summary>
    /// <param name="source">待加密的明文</param>
    /// <returns>加密后的字符串</returns>
    public static string Base64Encode(this string source)
    {
        return Base64Encode(source, Encoding.UTF8);
    }

    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="source">待加密的明文</param>
    /// <param name="encoding">加密采用的编码方式</param>
    /// <returns></returns>
    public static string Base64Encode(this string source, Encoding encoding)
    {
        string encode;
        byte[] bytes = encoding.GetBytes(source);
        try
        {
            encode = Convert.ToBase64String(bytes);
        }
        catch
        {
            encode = source;
        }

        return encode;
    }

    /// <summary>
    /// Base64解密
    /// 注:默认使用UTF8编码
    /// </summary>
    /// <param name="result">待解密的密文</param>
    /// <returns>解密后的字符串</returns>
    public static string Base64Decode(this string result)
    {
        return Base64Decode(result, Encoding.UTF8);
    }

    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="result">待解密的密文</param>
    /// <param name="encoding">解密采用的编码方式，注意和加密时采用的方式一致</param>
    /// <returns>解密后的字符串</returns>
    public static string Base64Decode(this string result, Encoding encoding)
    {
        string decode;
        byte[] bytes = Convert.FromBase64String(result);
        try
        {
            decode = encoding.GetString(bytes);
        }
        catch
        {
            decode = result;
        }

        return decode;
    }

    /// <summary>
    /// Base64Url编码
    /// </summary>
    /// <param name="text">待编码的文本字符串</param>
    /// <returns>编码的文本字符串</returns>
    public static string Base64UrlEncode(this string text)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(text);
        var base64 = Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');

        return base64;
    }

    /// <summary>
    /// Base64Url解码
    /// </summary>
    /// <param name="base64UrlStr">使用Base64Url编码后的字符串</param>
    /// <returns>解码后的内容</returns>
    public static string Base64UrlDecode(this string base64UrlStr)
    {
        base64UrlStr = base64UrlStr.Replace('-', '+').Replace('_', '/');
        switch (base64UrlStr.Length % 4)
        {
            case 2:
                base64UrlStr += "==";
                break;
            case 3:
                base64UrlStr += "=";
                break;
        }

        var bytes = Convert.FromBase64String(base64UrlStr);

        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// 计算SHA1摘要
    /// 注：默认使用UTF8编码
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static byte[] ToSha1Bytes(this string str)
    {
        return str.ToSha1Bytes(Encoding.UTF8);
    }

    /// <summary>
    /// 计算SHA1摘要
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="encoding">编码</param>
    /// <returns></returns>
    public static byte[] ToSha1Bytes(this string str, Encoding encoding)
    {
        using var sha1 = SHA1.Create();
        var inputBytes = encoding.GetBytes(str);
        return sha1.ComputeHash(inputBytes);
    }

    /// <summary>
    /// 转为SHA1哈希加密字符串
    /// 注：默认使用UTF8编码
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static string ToSha1String(this string str)
    {
        return str.ToSha1String(Encoding.UTF8);
    }

    /// <summary>
    /// 转为SHA1哈希
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="encoding">编码</param>
    /// <returns></returns>
    public static string ToSha1String(this string str, Encoding encoding)
    {
        byte[] sha1Bytes = str.ToSha1Bytes(encoding);
        string resStr = BitConverter.ToString(sha1Bytes);
        return resStr.Replace("-", "").ToLower();
    }

    /// <summary>
    /// SHA256加密
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static string ToSha256String(this string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        byte[] hash = SHA256.Create().ComputeHash(bytes);

        var builder = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            builder.Append(hash[i].ToString("x2"));
        }

        return builder.ToString();
    }

    /// <summary>
    /// HMACSHA256算法
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="secret">密钥</param>
    /// <returns></returns>
    public static string ToHmacsha256String(this string text, string secret)
    {
        secret ??= "";
        byte[] keyByte = Encoding.UTF8.GetBytes(secret);
        byte[] messageBytes = Encoding.UTF8.GetBytes(text);
        using (var hmacsha256 = new HMACSHA256(keyByte))
        {
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }
    }

    /// <summary>
    /// string转int
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        str = str.Replace("\0", "");
        if (string.IsNullOrEmpty(str))
            return 0;
        return Convert.ToInt32(str);
    }

    public static bool ToBool(this string str)
    {
        bool reval = false;
        if (!string.IsNullOrEmpty(str) && bool.TryParse(str, out reval))
        {
            return reval;
        }

        return reval;
    }

    /// <summary>
    /// string转long
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static long ToLong(this string str)
    {
        str = str.Replace("\0", "");
        if (string.IsNullOrEmpty(str))
            return 0;

        return Convert.ToInt64(str);
    }

    /// <summary>
    /// 二进制字符串转为Int
    /// </summary>
    /// <param name="str">二进制字符串</param>
    /// <returns></returns>
    public static int ToInt_FromBinString(this string str)
    {
        return Convert.ToInt32(str, 2);
    }

    /// <summary>
    /// 将16进制字符串转为Int
    /// </summary>
    /// <param name="str">数值</param>
    /// <returns></returns>
    public static int ToInt0X(this string str)
    {
        int num = Int32.Parse(str, NumberStyles.HexNumber);
        return num;
    }

    /// <summary>
    /// 转换为double
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static double ToDouble(this string str)
    {
        return Convert.ToDouble(str);
    }

    /// <summary>
    /// string转byte[]
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static byte[] ToBytes(this string str)
    {
        return Encoding.Default.GetBytes(str);
    }

    /// <summary>
    /// string转byte[]
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="theEncoding">需要的编码</param>
    /// <returns></returns>
    public static byte[] ToBytes(this string str, Encoding theEncoding)
    {
        return theEncoding.GetBytes(str);
    }

    /// <summary>
    /// 将16进制字符串转为Byte数组
    /// </summary>
    /// <param name="str">16进制字符串(2个16进制字符表示一个Byte)</param>
    /// <returns></returns>
    public static byte[] To0XBytes(this string str)
    {
        List<byte> resBytes = new List<byte>();
        for (int i = 0; i < str.Length; i = i + 2)
        {
            string numStr = $@"{str[i]}{str[i + 1]}";
            resBytes.Add((byte)numStr.ToInt0X());
        }

        return resBytes.ToArray();
    }

    /// <summary>
    /// 将ASCII码形式的字符串转为对应字节数组
    /// 注：一个字节一个ASCII码字符
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static byte[] ToAsciiBytes(this string str)
    {
        return str.ToList().Select(x => (byte)x).ToArray();
    }

    /// <summary>
    /// 转换为日期格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string str)
    {
        return Convert.ToDateTime(str);
    }

    /// <summary>
    /// 将Json字符串反序列化为对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="jsonStr">Json字符串</param>
    /// <returns></returns>
    public static T ToObject<T>(this string jsonStr)
    {
        return JsonConvert.DeserializeObject<T>(jsonStr);
    }

    /// <summary>
    /// 删除Json字符串中键中的@符号
    /// </summary>
    /// <param name="jsonStr">json字符串</param>
    /// <returns></returns>
    public static string RemoveAt(this string jsonStr)
    {
        Regex reg = new Regex("\"@([^ \"]*)\"\\s*:\\s*\"(([^ \"]+\\s*)*)\"");
        string strPatten = "\"$1\":\"$2\"";
        return reg.Replace(jsonStr, strPatten);
    }

    /// <summary>
    /// 将Json字符串反序列化为对象
    /// </summary>
    /// <param name="jsonStr">json字符串</param>
    /// <param name="type">对象类型</param>
    /// <returns></returns>
    public static object ToObject(this string jsonStr, Type type)
    {
        return JsonConvert.DeserializeObject(jsonStr, type);
    }

    /// <summary>
    /// 将XML字符串反序列化为对象
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="xmlStr">XML字符串</param>
    /// <returns></returns>
    public static T XmlStrToObject<T>(this string xmlStr)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlStr);
        string jsonJsonStr = JsonConvert.SerializeXmlNode(doc);

        return JsonConvert.DeserializeObject<T>(jsonJsonStr);
    }

    /// <summary>
    /// 将XML字符串反序列化为对象
    /// </summary>
    /// <param name="xmlStr">XML字符串</param>
    /// <returns></returns>
    public static JObject XmlStrToJObject(this string xmlStr)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlStr);
        string jsonJsonStr = JsonConvert.SerializeXmlNode(doc);

        return JsonConvert.DeserializeObject<JObject>(jsonJsonStr);
    }

    /// <summary>
    /// 将Json字符串转为List'T'
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="jsonStr"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(this string jsonStr)
    {
        return string.IsNullOrEmpty(jsonStr) ? null : JsonConvert.DeserializeObject<List<T>>(jsonStr);
    }

    /// <summary>
    /// 将Json字符串转为DataTable
    /// </summary>
    /// <param name="jsonStr">Json字符串</param>
    /// <returns></returns>
    public static DataTable ToDataTable(this string jsonStr)
    {
        return jsonStr == null ? null : JsonConvert.DeserializeObject<DataTable>(jsonStr);
    }

    /// <summary>
    /// 将Json字符串转为JObject
    /// </summary>
    /// <param name="jsonStr">Json字符串</param>
    /// <returns></returns>
    public static JObject ToJObject(this string jsonStr)
    {
        return jsonStr == null ? JObject.Parse("{}") : JObject.Parse(jsonStr.Replace("&nbsp;", ""));
    }

    /// <summary>
    /// 将Json字符串转为JArray
    /// </summary>
    /// <param name="jsonStr">Json字符串</param>
    /// <returns></returns>
    public static JArray ToJArray(this string jsonStr)
    {
        return jsonStr == null ? JArray.Parse("[]") : JArray.Parse(jsonStr.Replace("&nbsp;", ""));
    }

    /// <summary>
    /// json数据转实体类,仅仅应用于单个实体类，速度非常快
    /// </summary>
    /// <typeparam name="T">泛型参数</typeparam>
    /// <param name="json">json字符串</param>
    /// <returns></returns>
    public static T ToEntity<T>(this string json)
    {
        if (json == null || json == "")
            return default(T);

        var type = typeof(T);
        object obj = Activator.CreateInstance(type, null);

        foreach (var item in type.GetProperties())
        {
            if (obj != null)
            {
                PropertyInfo info = obj.GetType().GetProperty(item.Name);
                string pattern;
                pattern = "\"" + item.Name + "\":\"(.*?)\"";
                foreach (Match match in Regex.Matches(json, pattern))
                {
                    switch (item.PropertyType.ToString())
                    {
                        case "System.String":
                            if (info != null) info.SetValue(obj, match.Groups[1].ToString(), null);
                            break;
                        case "System.Int32":
                            if (info != null) info.SetValue(obj, match.Groups[1].ToString().ToInt(), null);
                            break;
                        case "System.Int64":
                            if (info != null) info.SetValue(obj, Convert.ToInt64(match.Groups[1].ToString()), null);
                            break;
                        case "System.DateTime":
                            if (info != null) info.SetValue(obj, Convert.ToDateTime(match.Groups[1].ToString()), null);
                            break;
                    }
                }
            }
        }

        return (T)obj;
    }

    /// <summary>
    /// 转为首字母大写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static string ToFirstUpperStr(this string str)
    {
        return str.Substring(0, 1).ToUpper() + str.Substring(1);
    }

    /// <summary>
    /// 转为首字母小写
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static string ToFirstLowerStr(this string str)
    {
        return str?.Substring(0, 1).ToLower() + str?.Substring(1);
    }

    /// <summary>
    /// 转为网络终结点IPEndPoint
    /// </summary>=
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static IPEndPoint ToIpEndPoint(this string str)
    {
        IPEndPoint iPEndPoint = null;
        try
        {
            string[] strArray = str.Split(':').ToArray();
            string addr = strArray[0];
            int port = Convert.ToInt32(strArray[1]);
            iPEndPoint = new IPEndPoint(IPAddress.Parse(addr), port);
        }
        catch
        {
            iPEndPoint = null;
        }

        return iPEndPoint;
    }

    /// <summary>
    /// 将枚举类型的文本转为枚举类型
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="enumText">枚举文本</param>
    /// <returns></returns>
    public static T ToEnum<T>(this string enumText)
    {
        var values = typeof(T).GetEnumValues().CastToList<T>();
        return values.Where(x => x.ToString() == enumText).FirstOrDefault();
    }

    /// <summary>
    /// 转为MurmurHash
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns></returns>
    public static uint ToMurmurHash(this string str)
    {
        return MurmurHash2.Hash(Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    /// 是否为弱密码
    /// 注:密码必须包含数字、小写字母、大写字母和其他符号中的两种并且长度大于8
    /// </summary>
    /// <param name="pwd">密码</param>
    /// <returns></returns>
    public static bool IsWeakPwd(this string pwd)
    {
        if (pwd.IsNullOrEmpty())
            throw new System.Exception("pwd不能为空");

        string pattern = "(^[0-9]+$)|(^[a-z]+$)|(^[A-Z]+$)|(^.{0,8}$)";
        if (Regex.IsMatch(pwd, pattern))
            return true;
        return false;
    }

    /// <summary>
    /// 判断是否邮箱
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsEmail(this string str)
    {
        Regex regEmail = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        Match m = regEmail.Match(str);
        return m.Success;
    }

    public static bool IsPhone(this string str)
    {
        return Regex.IsMatch(str, @"^[1]+[3,5]+\d{9}");
    }
}
