using Hold.API;
using Hold.API.Data;
using Hold.API.Endpoints;
using Hold.API.Services;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHoldDb(builder.Configuration);

builder.Services.AddScoped<Bank>();
builder.Services.AddRazorPages();
builder.Services.AddFeatureManagement();

var app = builder.Build();

app.MapRazorPages();

app.MapGroup("/hold")
   .MapHoldItemEndpoints()
   .WithFeatureGate(FeatureFlags.HoldItems);

app.MapGroup("/bank")
   .MapBankEndpoints()
   .WithFeatureGate(FeatureFlags.Bank);

Task.WaitAll(new [] {
  app.RunAsync(),
  app.PrepDBAsync()
});
