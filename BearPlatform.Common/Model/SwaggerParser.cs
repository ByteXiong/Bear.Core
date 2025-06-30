using System;
using System.IO;
using Newtonsoft.Json;

namespace BearPlatform.Common.Model
{


    public class SwaggerParser
    {
        public void ParseSwagger(string jsonPath)
        {
            // 读取 JSON 文件
            var json = File.ReadAllText(jsonPath);
            var document = JsonConvert.DeserializeObject<SwaggerDocument>(json);

            // 提取版本号
            var apiVersion = document.Info.Version;
            Console.WriteLine($"API Version: {apiVersion}");

            // 遍历所有接口路径
            foreach (var path in document.Paths)
            {
                var url = path.Key;
                var operations = path.Value;

                // 检查每个 HTTP 方法
                //ProcessOperation(operations.Get, "GET", url, apiVersion);
                //ProcessOperation(operations.Post, "POST", url, apiVersion);
                // 添加其他方法（PUT/DELETE等）...
            }
        }

        private void ProcessOperation(Operation operation, string method, string url, string apiVersion)
        {
            if (operation == null) return;

            Console.WriteLine($"接口信息:");
            Console.WriteLine($"- 版本: {apiVersion}");
            Console.WriteLine($"- 路径: {url}");
            Console.WriteLine($"- 方法: {method}");
            Console.WriteLine($"- 分组: {string.Join(",", operation.Tags)}");
            Console.WriteLine($"- 描述: {operation.Summary}");
            Console.WriteLine($"- 操作ID: {operation.OperationId}");

            // 如果使用自定义扩展属性存储版本
            if (!string.IsNullOrEmpty(operation.ApiVersion))
            {
                Console.WriteLine($"- 自定义版本: {operation.ApiVersion}");
            }
            Console.WriteLine("-------------------");
        }
    }
}
