using Hold.API.Data;
using Hold.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Hold.API.Services;

public class Bank(
  ILogger<Bank> logger,
  HoldDbContext db
) {

  private readonly ILogger _logger = logger;
  private readonly HoldDbContext _db = db;

  public IEnumerable<BankAccount> Accounts => _db.Accounts.ToArray();

  public async Task<BankAccount> AddAccountAsync(string name, float startingAmount = 0.0f, string description = "Initial Deposit") {
    var account = (await _db.Accounts.AddAsync(new BankAccount {
      Name = name
    })).Entity;

    if(startingAmount != 0.0f)
      account.Deposit(startingAmount, description);

    return account;
  }

  public BankAccount? TryGetAccount(Guid id) =>
    _db.Accounts
    .Include(e => e.Transactions)
    .FirstOrDefault(e => e.ID == id);

  public void Save() =>
    _db.SaveChanges();

  public Task SaveAsync() =>
    _db.SaveChangesAsync();

}
