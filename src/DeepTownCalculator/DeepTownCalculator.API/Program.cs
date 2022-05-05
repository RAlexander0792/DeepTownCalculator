using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using DeepTownCalculator.Application;
using DeepTownCalculator.Application.Interfaces;
using DeepTownCalculator.Data.File.Repositories;
using DeepTownCalculator.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

ConfigureRepositories(builder.Services);
ConfigureServices(builder.Services);

builder.Host.UseMetricsWebTracking()
    .UseMetrics( (options) => {
        options.EndpointOptions = endpointOptions =>
            {
                endpointOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                endpointOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                endpointOptions.EnvironmentInfoEndpointEnabled = false;
            };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<ICalculatorService, CalculatorService>();
}

void ConfigureRepositories(IServiceCollection services)
{
    services.AddSingleton<IAreaRepository, AreaRepository>();
    services.AddSingleton<IExtractorsRepository, ExtractorsRepository>();
    services.AddSingleton<IRecipesRepository, RecipesRepository>();
    services.AddSingleton<IBuildingsRepository, BuildingsRepository>();
}