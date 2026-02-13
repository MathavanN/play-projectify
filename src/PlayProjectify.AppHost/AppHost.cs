var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak", 8080);

var apiService = builder.AddProject<Projects.PlayProjectify_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var productService = builder.AddProject<Projects.PlayProjectify_ProductService>("productservice")
    .WithHttpHealthCheck("/health");

var orderService = builder.AddProject<Projects.PlayProjectify_OrderService>("orderservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.PlayProjectify_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WithReference(productService)
    .WaitFor(apiService)
    .WaitFor(productService);

builder.Build().Run();
