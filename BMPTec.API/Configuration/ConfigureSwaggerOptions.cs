using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BMPTec.API.Configuration
{

    /// <summary>
    /// Configuração do Swagger para suportar versionamento
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "API BMP Tec S.A.",
                Version = description.ApiVersion.ToString(),
                Description = "API para gerenciamento de contas e transferências bancárias do BMP Tec S.A.",
                Contact = new OpenApiContact
                {
                    Name = "BMPTec",
                    Email = "moneyp.com.br"
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " Esta versão da API está obsoleta.";
            }

            return info;
        }
    }
}
