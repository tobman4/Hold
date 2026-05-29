namespace Hold.API.Data.Models;

public class BankTransaction {
  public int ID { get; init; }
  public Guid AccountID { get; init; }

  public float Ammount { get; init; }
  public string Type { get; init; } = string.Empty;

  public BankAccount Account { get; init; } = null!;
}
