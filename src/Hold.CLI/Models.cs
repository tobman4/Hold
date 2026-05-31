namespace Hold.CLI;

public record BankAccountDto(Guid id, string name, float balance);
public record BankTransactionDto(Guid id, float amount, string type, string description);
