var builder = DistributedApplication.CreateBuilder(args);

var isHttpEndpoints = ShouldUseHttpForEndpoints();
//var keycloak = builder.AddKeycloak("keycloak", 8080);

var apiService = builder.AddProject<Projects.PlayProjectify_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var productService = builder.AddProject<Projects.PlayProjectify_ProductService>("productservice")
    .WithHttpHealthCheck("/health");

var orderService = builder.AddProject<Projects.PlayProjectify_OrderService>("orderservice")
    .WithHttpHealthCheck("/health")
    .WithReference(productService)
    .WaitFor(productService);

var web = builder.AddProject<Projects.PlayProjectify_Web>("webfrontend")
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WithReference(productService)
    .WithReference(orderService)
    .WaitFor(apiService)
    .WaitFor(productService)
    .WaitFor(orderService);

if (!isHttpEndpoints) web.WithExternalHttpEndpoints();

builder.Build().Run();

static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "PROJECTIFY_USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}
