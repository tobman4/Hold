using Microsoft.EntityFrameworkCore;
using Hold.API.Data.Models;

namespace Hold.API.Data;

public class HoldDbContext : DbContext {
  public HoldDbContext(DbContextOptions<HoldDbContext> options) : base(options) {
  }

  public DbSet<HoldItem> HoldItems { get; set; } = null!;
  public DbSet<BankAccount> Accounts { get; set; } = null!;
  public DbSet<BankTransaction> Transactions { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {

    modelBuilder.Entity<BankAccount>()
      .ToTable("Account");

    modelBuilder.Entity<BankAccount>()
      .HasKey(e => e.ID);

    modelBuilder.Entity<BankAccount>()
      .HasAlternateKey(e => e.Name);

    modelBuilder.Entity<BankAccount>()
      .HasMany(e => e.Transactions)
      .WithOne(e => e.Account)
      .HasForeignKey(e => e.AccountID);
  }
}
