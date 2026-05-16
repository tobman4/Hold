namespace Hold.API.Data.Models;

public class HoldItem {
  
  public Guid ID { get; init; } = Guid.NewGuid();
  public string Name { get; set; } = null!;
  public string? Description { get; set; }

  public int Count { get; set; } = 0;
}
