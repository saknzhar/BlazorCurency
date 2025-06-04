using MoneyRate.Interfaces;
using MoneyRate.Data;
using MoneyRate.Interfaces.Dtos;

namespace MoneyRate.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyApiClient _currencyApiClient;
        public CurrencyService(ICurrencyApiClient currencyApiClient)
        {
            _currencyApiClient = currencyApiClient;
        }

        public async Task<CurrencyRatesResponse> GetCurrencyRatesAsync()
        {
            var currencyRates = await _currencyApiClient.GetDetailedRatesAsync();

            return currencyRates;
        }

        public async Task<CurrencyRateDetail> GetCurrencyRateAsync(string currencyCode)
        {
            var currencyRates = await GetCurrencyRatesAsync();

            return currencyRates.CurrencyRatesList.Where(x => x.CurrencyCode == currencyCode).FirstOrDefault();

        }
    }
}