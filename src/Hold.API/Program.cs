using Hold.API.Data;
using Hold.API.Endpoints;
using Hold.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHoldDb(builder.Configuration);

builder.Services.AddScoped<Bank>();
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapRazorPages();

app.MapHoldItemEndpoints();
app.MapBankEndpoints();

Task.WaitAll(new [] {
  app.RunAsync(),
  app.PrepDBAsync()
});
