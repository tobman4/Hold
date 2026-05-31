namespace Hold.API.Data.Models;

public class BankAccount { 

  public Guid ID { get; init; } = Guid.NewGuid();
  public string Name { get; set; } = null!;

  public float Balance { get; set; } = 0.0f;

  public ICollection<BankTransaction> Transactions { get; set; } = new List<BankTransaction>();


  public void Deposit(float amount, string description = "") {
    if(amount <= 0)
      throw new ArgumentException();

    Balance += amount;
    Transactions.Add(new BankTransaction {
      AccountID = this.ID,
      Amount = amount,
      Type = "Deposit",
      Description = description
    });

  }


  public void Withdraw(float amount, string description = "") {
    if(amount <= 0)
      throw new ArgumentException();

    Balance -= amount;
    Transactions.Add(new BankTransaction {
      AccountID = this.ID,
      Amount = amount,
      Type = "Withdraw",
      Description = description
    });
  }

}
