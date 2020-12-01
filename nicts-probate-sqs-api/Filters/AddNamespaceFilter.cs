using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using nicts_probate_sqs_api.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace nicts_probate_sqs_api.Filters
{
    public class AddNamespaceFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Type != "object") return;
            if (context.Type == typeof(ProbateApplicationModel))
            {
                schema.Xml = new OpenApiXml()
                {
                    Namespace = new Uri("http://schemas.datacontract.org/2004/07/nicts_probate_sqs_api.Models")
                };
            }
        }
    }
}
