using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ASCOM.Alpaca
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

            var keys = new System.Collections.Generic.List<string>();
            var prefix = "System";

            if (schemaRegistry.MemberInfo?.DeclaringType.FullName.Contains("System.") ?? false)
            {
                foreach (var key in schemaRegistry.SchemaRepository.Schemas.Keys)
                {
                    if (!key.Contains("Alpaca"))
                    {
                        keys.Add(key);
                    }

                    if (key.Equals("Exception"))
                    {
                        keys.Add(key);
                    }
                }
            }
            foreach (var key in keys)
            {
                schemaRegistry.SchemaRepository.Schemas.Remove(key);
            }
        }
    }
}
