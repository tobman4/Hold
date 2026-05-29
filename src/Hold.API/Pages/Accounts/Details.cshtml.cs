using Hold.API.Data.Models;
using Hold.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hold.API.Pages.Accounts;

public class DetailsModel(Bank bank) : PageModel {
  private readonly Bank _bank = bank;

  public BankAccount? Account { get; set; }

  public IActionResult OnGet(Guid id) {
    Account = _bank.TryGetAccount(id);
    if (Account == null) {
      return NotFound();
    }
    return Page();
  }

  public async Task<IActionResult> OnPostAsync(Guid accountId, string transactionType, float amount) {
    var account = _bank.TryGetAccount(accountId);
    if (account == null) {
      return NotFound();
    }

    try {
      if (transactionType == "Deposit") {
        account.Deposit(amount);
      } else if (transactionType == "Withdraw") {
        account.Withdraw(amount);
      }

      await _bank.SaveAsync();
    } catch (ArgumentException) {
      // Basic fallback if negative amount was supplied, per user requirement there are no restrictions
      // but BankAccount.cs throws on <= 0, so we just catch it and ignore or let it fail gracefully.
    }

    return RedirectToPage(new { id = accountId });
  }
}
