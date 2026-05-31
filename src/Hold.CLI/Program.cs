using Hold.CLI;
using System.CommandLine;

var app = new CliApp("Hold CLI Tool");
var client = new BankClient("http://localhost:5272");

app.MapDefault(() => 
{
    Console.WriteLine("Welcome to Hold CLI! Use --help to see available commands.");
});

var bank = app.MapCommand("bank", () => 
{
    Console.WriteLine("Bank management. Use --help to see subcommands.");
}, "Bank management commands");

bank.MapCommand("list", async () => 
{
    var accounts = await client.GetAccountsAsync();
    Console.WriteLine("Accounts:");
    foreach (var acc in accounts)
    {
        Console.WriteLine($"- {acc.name} ({acc.id}): {acc.balance:C}");
    }
}, "List all bank accounts");

bank.MapCommand("add", async (string name, float startingAmount = 0, string description = "Initial Deposit") => 
{
    var acc = await client.AddAccountAsync(name, startingAmount, description);
    Console.WriteLine($"Account created: {acc?.name} ({acc?.id}) with balance {acc?.balance:C}");
}, "Add a new bank account");

bank.MapCommand("view", async (Guid id) => 
{
    var acc = await client.GetAccountAsync(id);
    if (acc == null) { Console.WriteLine("Account not found."); return; }
    Console.WriteLine($"Account: {acc.name}");
    Console.WriteLine($"ID:      {acc.id}");
    Console.WriteLine($"Balance: {acc.balance:C}");
}, "View account details");

bank.MapCommand("update", async (Guid id, float amount, string description = "") => 
{
    var acc = await client.UpdateAccountAsync(id, amount, description);
    Console.WriteLine($"Account updated. New balance: {acc?.balance:C}");
}, "Deposit (positive) or withdraw (negative) funds");

bank.MapCommand("transactions", async (Guid id) => 
{
    var transactions = await client.GetTransactionsAsync(id);
    Console.WriteLine("Transactions:");
    foreach (var t in transactions)
    {
        Console.WriteLine($"- {t.type}: {t.amount:C} - {t.description} ({t.id})");
    }
}, "List transactions for an account");

return await app.RunAsync(args);
