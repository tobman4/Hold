using Hold.API.Data.Models;
using Hold.API.Services;
using Microsoft.AspNetCore.Mvc;


namespace Hold.API.Endpoints;


static class BankEndpoint {
  public static T MapBankEndpoints<T>(this T app) where T : IEndpointRouteBuilder {
    app.MapGet("", GetAll);
    app.MapPost("", AddAccount);

    app.MapGet("{id}", GetAccount);
    app.MapPut("{id}", Update);

    app.MapGet("{id}/transitions", GetTransitions);


    return app;
  }

  private static IEnumerable<BankAccount> GetAll([FromServices]Bank bank) =>
    bank.Accounts;

  private static IResult GetAccount([FromServices]Bank bank, Guid id) {
    if(bank.TryGetAccount(id) is not BankAccount acc)
      return Results.NotFound();

    return Results.Ok(acc);
  }

  private static IResult GetTransitions([FromServices]Bank bank, Guid id) {
    if(bank.TryGetAccount(id) is not BankAccount acc)
      return Results.NotFound();
    
    return Results.Ok(acc.Transitions.ToArray());
  }

  private static async Task<IResult> AddAccount([FromServices]Bank bank, [FromQuery]string name, [FromQuery]float startingAmmount = 0.0f, [FromQuery]string description = "Initial Deposit") {
    if(string.IsNullOrWhiteSpace(name))
      return Results.BadRequest("Bad account name");

    var acc = await bank.AddAccountAsync(name, startingAmmount, description);

    await bank.SaveAsync();
    return Results.Ok(acc);
  }

  private static IResult Update([FromServices]Bank bank, Guid id, [FromQuery]float ammout, [FromQuery]string description = "") {
    if(bank.TryGetAccount(id) is not BankAccount acc)
      return Results.NotFound();

    if(ammout < 0.0f)
      acc.Withdraw(Math.Abs(ammout), description);

    else if(ammout > 0.0f)
      acc.Deposit(ammout, description);

    bank.Save();
    return Results.Ok(acc);
  }

}
