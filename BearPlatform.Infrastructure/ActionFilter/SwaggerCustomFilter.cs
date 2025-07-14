using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BearPlatform.Infrastructure.ActionFilter;
public class CustomSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(long))
        {
            schema.Type = "string";
            schema.Format = null;
        }
        else if (context.Type == typeof(long?))
        {
            schema.Type = "string";
            schema.Format = null;
            schema.Nullable = true;
        }
      
       
    }

}
