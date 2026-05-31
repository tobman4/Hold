# Hold.CLI

A modern, Minimal API-style CLI tool for managing the **Hold** ecosystem. Built with `System.CommandLine`, it provides a clean and fluent way to define and execute commands.

## Features

- **Minimal API Style:** Define commands using a familiar `app.MapCommand("name", action)` syntax.
- **Automatic Option Mapping:** Delegate parameters are automatically converted to CLI options (e.g., `string name` becomes `--name`).
- **Subcommand Support:** Easily nest commands for complex CLI structures.
- **Bank Management:** Full integration with `Hold.API` to list accounts, add funds, and view transactions.

## Getting Started

### Prerequisites

- .NET 10.0 SDK
- Running instance of `Hold.API` (default: `http://localhost:5272`)

### Installation

To install the tool globally from the source:

```bash
dotnet pack Hold.CLI/Hold.CLI.csproj
dotnet tool install --global --add-source Hold.CLI/nupkg Hold.CLI
```

## Usage

### Basic Commands

```bash
# Show help
hold --help

# Default welcome message
hold

# Sample greet command
hold greet --name "User" --count 3
```

### Bank Management

The `bank` command group allows you to manage your accounts:

| Command | Description |
| --- | --- |
| `hold bank list` | List all accounts and balances |
| `hold bank add` | Create a new account |
| `hold bank view` | View account details |
| `hold bank update` | Deposit or withdraw funds |
| `hold bank transactions` | View transaction history |

**Examples:**

```bash
# Add a new account
hold bank add --name "Savings" --starting-amount 1000

# Deposit money
hold bank update --id <GUID> --amount 500 --description "Bonus"

# Withdraw money
hold bank update --id <GUID> --amount -50 --description "Coffee"
```

## Developing with the CliApp Framework

The `CliApp` framework allows you to add new commands in seconds:

```csharp
var app = new CliApp("My Tool");

app.MapCommand("do-work", (string task, int priority = 1) => 
{
    Console.WriteLine($"Doing {task} with priority {priority}");
}, "Performs a task");

await app.RunAsync(args);
```

### How it works:
- **Naming:** Parameters like `startingAmount` are converted to kebab-case: `--starting-amount`.
- **Types:** Supports standard .NET types (string, int, float, Guid, etc.) with automatic parsing and validation.
- **Defaults:** Optional parameters in your delegate automatically become optional CLI options with the same default value.
