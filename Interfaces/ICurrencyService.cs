using MoneyRate.Data;
using MoneyRate.Interfaces.Dtos;

namespace MoneyRate.Interfaces
{
    public interface ICurrencyService
    {
        Task<CurrencyRatesResponse> GetCurrencyRatesAsync();

        Task<CurrencyRateDetail> GetCurrencyRateAsync(string currencyCode);
    }
}