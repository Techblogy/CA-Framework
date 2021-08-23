using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAF.UI.WebApi.Filters
{
    public class SwaggerEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                foreach (var n in Enum.GetValues(context.Type))
                {
                    schema.Enum.Add(new OpenApiString(n + $" = {Convert.ToInt32(n)}"));
                }
            }
        }
    }
}
