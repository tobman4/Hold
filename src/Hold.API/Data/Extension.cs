using Microsoft.EntityFrameworkCore;

namespace Hold.API.Data;

public static class Extension {
  public static IServiceCollection AddHoldDb(this IServiceCollection services, IConfiguration configuration) {
    var dbPath = configuration["DB"];
    if (string.IsNullOrEmpty(dbPath))
      throw new ArgumentNullException(nameof(dbPath), "Database path not configured in 'DB' setting.");

    services.AddDbContext<HoldDbContext>(options =>
      options.UseSqlite($"Data Source={dbPath}"));

    return services;
  }

  public static async Task PrepDBAsync(this IHost app) {
    using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<HoldDbContext>();

    var pending = await db.Database.GetPendingMigrationsAsync();
    if(pending.Count() > 0)
      await db.Database.MigrateAsync();
  }
}
