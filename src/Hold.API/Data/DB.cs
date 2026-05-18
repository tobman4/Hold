using Microsoft.EntityFrameworkCore;
using Hold.API.Data.Models;

namespace Hold.API.Data;

public class HoldDbContext : DbContext {
  public HoldDbContext(DbContextOptions<HoldDbContext> options) : base(options) {
  }

  public DbSet<HoldItem> HoldItems { get; set; } = null!;
}
