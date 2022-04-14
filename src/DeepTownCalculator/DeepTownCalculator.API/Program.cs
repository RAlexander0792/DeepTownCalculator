using DeepTownCalculator.Application;
using DeepTownCalculator.Application.Interfaces;
using DeepTownCalculator.Data.File.Repositories;
using DeepTownCalculator.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

ConfigureRepositories(builder.Services);
ConfigureServices(builder.Services);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICalculatorService, CalculatorService>();

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
}