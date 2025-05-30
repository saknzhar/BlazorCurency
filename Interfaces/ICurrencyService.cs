using MoneyRate.Data;

namespace MoneyRate.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<CurrencyRate>> GetCurrencyRatesAsync();
        Task<CurrencyRate?> GetCurrencyRateByIdAsync(int id);
        Task AddCurrencyRateAsync(CurrencyRate rate);
        Task UpdateCurrencyRateAsync(CurrencyRate rate);
        Task DeleteCurrencyRateAsync(int id);
    }
}