namespace Hold.API.Endpoints;

static class Test {

  private static readonly TestItem _item = new();

  public static void MapTest(this WebApplication app) {
    app.MapGet("t", () => _item);
    app.MapPut("t", Update);
  }

  private static void Update(HttpContext ctx) {

    var args = ctx.Request.Query;
    foreach(var a in args)
      Console.WriteLine($"{a.Key}: {a.Value}");

  }

}



class CanWriteAttribute : Attribute {}

class TestItem {
  
  [CanWrite]
  public int Test { get; set; }

}
