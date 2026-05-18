using Hold.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHoldDb(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

Task.WaitAll(new [] {
  app.RunAsync(),
  app.PrepDBAsync()
});
