using System.Net.Http.Json;

namespace Hold.CLI;

public class BankClient
{
    private readonly HttpClient _httpClient;

    public BankClient(string baseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    public async Task<IEnumerable<BankAccountDto>> GetAccountsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<BankAccountDto>>("bank") ?? Array.Empty<BankAccountDto>();
    }

    public async Task<BankAccountDto?> GetAccountAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<BankAccountDto>($"bank/{id}");
    }

    public async Task<BankAccountDto?> AddAccountAsync(string name, float startingAmount, string description)
    {
        var response = await _httpClient.PostAsync($"bank?name={Uri.EscapeDataString(name)}&startingAmount={startingAmount}&description={Uri.EscapeDataString(description)}", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BankAccountDto>();
    }

    public async Task<BankAccountDto?> UpdateAccountAsync(Guid id, float amount, string description)
    {
        var response = await _httpClient.PutAsync($"bank/{id}?amount={amount}&description={Uri.EscapeDataString(description)}", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BankAccountDto>();
    }

    public async Task<IEnumerable<BankTransactionDto>> GetTransactionsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<BankTransactionDto>>($"bank/{id}/transactions") ?? Array.Empty<BankTransactionDto>();
    }
}
