using System.Collections.Generic;
using Newtonsoft.Json;


namespace BearPlatform.Common.Model
{

    public class SwaggerDocument
    {
        [JsonProperty("openapi")]
        public string OpenApiVersion { get; set; }

        [JsonProperty("info")]
        public ApiInfo Info { get; set; }

        [JsonProperty("paths")]
        public Dictionary<string, Dictionary<string, Operation>> Paths { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }

    public class ApiInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class PathItem
    {

        public Dictionary<string, Operation> Items { get; set; }
        //[JsonProperty("get")]
        //public Operation Get { get; set; }

        //[JsonProperty("post")]
        //public Operation Post { get; set; }

        // 其他 HTTP 方法同理
    }

    public class Operation
    {
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("operationId")]
        public string OperationId { get; set; }

        [JsonProperty("x-api-version")] // 自定义扩展属性示例
        public string ApiVersion { get; set; }
    }

    public class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
