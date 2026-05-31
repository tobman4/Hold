namespace Hold.API.Data.Models;

public class Post {
  public Guid ID { get; set; } = Guid.NewGuid();
  public required string Title { get; set; }
  public required string Body { get; set; }
  public DateTime Time { get; set; } = DateTime.UtcNow;
}
