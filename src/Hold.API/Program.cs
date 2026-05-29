using Hold.API.Data;
using Hold.API.Endpoints;
using Hold.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHoldDb(builder.Configuration);

builder.Services.AddScoped<Bank>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHoldItemEndpoints();
app.MapBankEndpoints();

Task.WaitAll(new [] {
  app.RunAsync(),
  app.PrepDBAsync()
});
