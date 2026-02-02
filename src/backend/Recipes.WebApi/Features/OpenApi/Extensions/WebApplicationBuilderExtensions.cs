using System.Text.Json.Serialization;
using Recipes.WebApi.Features.OpenApi.Transformers;
using Scalar.AspNetCore;

namespace Recipes.WebApi.Features.OpenApi.Extensions;

internal static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddOpenApiSpecification(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi("v1", opt =>
        {
            opt.AddDocumentTransformer<ProjectDocumentTransformer>();
            opt.AddSchemaTransformer<NumericSchemaTransformer>();
        });

        // Configure HTTP JSON options with JsonStringEnumConverter so OpenAPI spec
        // generates string enum values (e.g., "Small", "Medium", "Large") instead of integers.
        // This is separate from AddControllers().AddJsonOptions() which handles API responses.
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return builder;
    }

    public static void UseOpenApiDocumentation(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(opt =>
        {
            opt.WithTitle("Recipes API");
            opt.WithTheme(ScalarTheme.Mars);
            opt.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            opt.WithOperationTitleSource(OperationTitleSource.Path);
            opt.SortTagsAlphabetically();
            opt.WithSearchHotKey("k");
        });
    }
}
