using MoneyRate.Data;
using MoneyRate.Interfaces.Dtos;

namespace MoneyRate.Interfaces
{
    public interface ICurrencyService
    {
        Task<CurrencyRatesResponse> GetCurrencyRatesAsync();
        Task<CurrencyRate?> GetCurrencyRateByIdAsync(int id);
        Task AddCurrencyRateAsync(CurrencyRate rate);
        Task UpdateCurrencyRateAsync(CurrencyRate rate);
        Task DeleteCurrencyRateAsync(int id);
    }
}