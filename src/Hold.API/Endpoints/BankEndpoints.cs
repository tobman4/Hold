using Hold.API.Data.Models;
using Hold.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;


namespace Hold.API.Endpoints;


static class BankEndpoint {
  public static T MapBankEndpoints<T>(this T app) where T : IEndpointRouteBuilder {
    app.MapGet("", GetAll);
    app.MapPost("", AddAccount);

    app.MapGet("{id}", GetAccount);
    app.MapPut("{id}", Update);

    app.MapGet("{id}/transactions", GetTransactions);
    app.MapGet("{id}/export", ExportTransactions);


    return app;
  }

  private static IEnumerable<BankAccount> GetAll([FromServices]Bank bank) =>
    bank.Accounts;

  private static IResult GetAccount([FromServices]Bank bank, Guid id) {
    if(bank.TryGetAccount(id) is not BankAccount acc)
      return Results.NotFound();

    return Results.Ok(acc);
  }

  private static IResult GetTransactions([FromServices]Bank bank, Guid id) {
    if(bank.TryGetAccount(id) is not BankAccount acc)
      return Results.NotFound();
    
    return Results.Ok(acc.Transactions.ToArray());
  }

  private static IResult ExportTransactions([FromServices]Bank bank, Guid id) {
    if (bank.TryGetAccount(id) is not BankAccount acc)
      return Results.NotFound();

    var csv = new StringBuilder();
    csv.AppendLine("ID,Amount,Type,Description");

    foreach (var t in acc.Transactions) {
      csv.AppendLine($"{t.ID},{t.Amount},{t.Type},\"{t.Description.Replace("\"", "\"\"")}\"");
    }

    return Results.File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"account_{id}_history.csv");
  }

  private static async Task<IResult> AddAccount([FromServices]Bank bank, [FromQuery]string name, [FromQuery]float startingAmount = 0.0f, [FromQuery]string description = "Initial Deposit") {
    if(string.IsNullOrWhiteSpace(name))
      return Results.BadRequest("Bad account name");

    var acc = await bank.AddAccountAsync(name, startingAmount, description);

    await bank.SaveAsync();
    return Results.Ok(acc);
  }

  private static IResult Update([FromServices]Bank bank, Guid id, [FromQuery]float amount, [FromQuery]string description = "") {
    if(bank.TryGetAccount(id) is not BankAccount acc)
      return Results.NotFound();

    if(amount < 0.0f)
      acc.Withdraw(Math.Abs(amount), description);

    else if(amount > 0.0f)
      acc.Deposit(amount, description);

    bank.Save();
    return Results.Ok(acc);
  }

}
