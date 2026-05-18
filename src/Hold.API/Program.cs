using Hold.API.Data;
using Hold.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHoldDb(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHoldItemEndpoints();

Task.WaitAll(new [] {
  app.RunAsync(),
  app.PrepDBAsync()
});
