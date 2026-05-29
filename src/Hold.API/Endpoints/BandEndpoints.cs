using Hold.API.Data.Models;
using Hold.API.Services;
using Microsoft.AspNetCore.Mvc;


namespace Hold.API.Endpoints;


static class BankEndpoint {
  public static WebApplication MapBankEndpoints(this WebApplication app) {
    var group = app.MapGroup("/bank");

    group.MapGet("", GetAll);
    group.MapPost("", AddAccount);

    group.MapGet("{id}", GetAccount);
    group.MapPut("{id}", Update);

    group.MapGet("{id}/transitions", GetTransitions);


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

  private static async Task<IResult> AddAccount([FromServices]Bank bank, [FromQuery]string name) {
    if(string.IsNullOrWhiteSpace(name))
      return Results.BadRequest("Bad account name");

    var acc = await bank.AddAccountAsync(name);

    await bank.SaveAsync();
    return Results.Ok(acc);
  }

  private static IResult Update([FromServices]Bank bank, Guid id, float ammout) {
    if(bank.TryGetAccount(id) is not BankAccount acc)
      return Results.NotFound();

    if(ammout < 0.0f)
      acc.Withdraw(Math.Abs(ammout));

    else if(ammout > 0.0f)
      acc.Deposit(ammout);

    bank.Save();
    return Results.Ok(acc);
  }

}
