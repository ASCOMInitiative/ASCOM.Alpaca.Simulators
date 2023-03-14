using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ASCOM.Alpaca.Simulators
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext schemaRegistry)
        {
            if (schema?.Properties == null || schemaRegistry.Type == null)
                return;

            // DriverException is part of a response but not part of the Alpaca API
            var excludedProperties = schemaRegistry.Type.GetProperties()
                                         .Where(t => t.Name == "DriverException");

            foreach (var excludedProperty in excludedProperties)
            {
                if (schema.Properties.ContainsKey(excludedProperty.Name))
                    schema.Properties.Remove(excludedProperty.Name);
            }
        }
    }
}
