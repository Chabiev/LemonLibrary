using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.SwaggerFilters;

public class AddFileUploadConsumesAttribute : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters != null)
        {
            var fileUploadParameter = context.ApiDescription.ParameterDescriptions
                .FirstOrDefault(p => p.ModelMetadata.ModelType == typeof(IFormFile));

            if (fileUploadParameter != null)
            {
                if (!operation.RequestBody.Content.ContainsKey("multipart/form-data"))
                {
                    operation.RequestBody.Content.Add(
                        "multipart/form-data",
                        new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    { fileUploadParameter.Name, new OpenApiSchema { Type = "string", Format = "binary" } }
                                }
                            },
                            Encoding = new Dictionary<string, OpenApiEncoding>
                            {
                                {
                                    fileUploadParameter.Name,
                                    new OpenApiEncoding
                                    {
                                        ContentType = "multipart/form-data"
                                    }
                                }
                            }
                        }
                    );
                }
            }
        }
    }
}