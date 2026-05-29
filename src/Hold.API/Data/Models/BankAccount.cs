namespace Hold.API.Data.Models;

public class BankAccount { 

  public Guid ID { get; init; } = Guid.NewGuid();
  public string Name { get; set; } = null!;

  public float Balance { get; set; } = 0.0f;

  public ICollection<BankTransaction> Transitions = new List<BankTransaction>();


  public void Deposit(float ammount) {
    if(ammount <= 0)
      throw new ArgumentException();

    Balance += ammount;
    Transitions.Add(new BankTransaction {
      AccountID = this.ID,
      Ammount = ammount,
      Type = "Deposit"
    });

  }


  public void Withdraw(float ammount) {
    if(ammount <= 0)
      throw new ArgumentException();

    Balance -= ammount;
    Transitions.Add(new BankTransaction {
      AccountID = this.ID,
      Ammount = ammount,
      Type = "Withdraw"
    });
  }

}
