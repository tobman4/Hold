using System.Text.Json.Serialization;

namespace Hold.API.Data.Models;

public class BankTransaction {
  public int ID { get; init; }
  public Guid AccountID { get; init; }

  public float Amount { get; init; }
  public string Type { get; init; } = string.Empty;
  public string Description { get; init; } = string.Empty;

  [JsonIgnore]
  public BankAccount Account { get; init; } = null!;
}
