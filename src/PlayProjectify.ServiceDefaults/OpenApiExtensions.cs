using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace PlayProjectify.ServiceDefaults;

public static class OpenApiExtensions
{
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app, string[] versions)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection("OpenApi");

        if (!openApiSection.Exists())
            return app;

        app.MapOpenApi();
        List<ScalarDocument> documents = [];
        foreach (var desc in versions)
            documents.Add(new ScalarDocument(desc, $"{openApiSection.GetRequiredValue("Document:Title")} {desc.ToUpper()}", $"/openapi/{desc}.json"));

        if (app.Environment.IsDevelopment())
        {

            app.MapScalarApiReference(options =>
            {
                // Disable default fonts to avoid download unnecessary fonts
                options.DefaultFonts = false;
                options.WithTitle(openApiSection.GetRequiredValue("Document:Title"));
                options.WithTheme(ScalarTheme.BluePlanet);
                options.EnableDarkMode();
                options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                options.AddDocuments(documents);

            });


            //app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
        }

        return app;
    }

    public static IHostApplicationBuilder AddDefaultOpenApi(this IHostApplicationBuilder builder, string[] versions)
    {
        var openApi = builder.Configuration.GetSection("OpenApi");
        var identitySection = builder.Configuration.GetSection("Identity");

        var scopes = identitySection.Exists()
            ? identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value)
            : [];

        if (!openApi.Exists())
            return builder;
        foreach (var version in versions)
        {
            builder.Services.AddOpenApi(version, options =>
            {
                options.ApplyApiVersionInfo(openApi.GetRequiredValue("Document:Title"), openApi.GetRequiredValue("Document:Description"));
                //options.ApplyAuthorizationChecks([.. scopes.Keys]);
                //options.ApplySecuritySchemeDefinitions();
                options.ApplyOperationDeprecatedStatus();
                options.ApplyApiVersionDescription();
            });
        }

        return builder;
    }
}