var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.PlayProjectify_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.PlayProjectify_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.PlayProjectify_ProductService>("productservice");

builder.Build().Run();
