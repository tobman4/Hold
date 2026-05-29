using Hold.API.Data.Models;
using Hold.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hold.API.Pages.Accounts;

public class IndexModel(Bank bank) : PageModel {
  private readonly Bank _bank = bank;

  public IEnumerable<BankAccount> Accounts { get; set; } = Array.Empty<BankAccount>();

  public void OnGet() {
    Accounts = _bank.Accounts;
  }

  public async Task<IActionResult> OnPostAsync(string name, float startingBalance) {
    if (string.IsNullOrWhiteSpace(name)) {
      return RedirectToPage();
    }

    await _bank.AddAccountAsync(name, startingBalance);
    await _bank.SaveAsync();

    return RedirectToPage();
  }
}
