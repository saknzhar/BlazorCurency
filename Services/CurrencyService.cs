using MoneyRate.Interfaces;
using MoneyRate.Data;
using MoneyRate.Interfaces.Dtos;

namespace MoneyRate.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyApiClient _currencyApiClient;
        public CurrencyService( ICurrencyApiClient currencyApiClient)
        {
            _currencyApiClient = currencyApiClient;
        }
        // Имитируем базу данных в памяти
        private static List<CurrencyRate> _currencyRates = new List<CurrencyRate>
        {
            new CurrencyRate { Id = 1, CurrencyCode = "USD", CurrencyName = "Доллар США", Rate = 89.50m, Date = DateTime.Now.AddDays(-1) },
            new CurrencyRate { Id = 2, CurrencyCode = "EUR", CurrencyName = "Евро", Rate = 97.25m, Date = DateTime.Now.AddDays(-1) },
            new CurrencyRate { Id = 3, CurrencyCode = "GBP", CurrencyName = "Фунт стерлингов", Rate = 112.10m, Date = DateTime.Now.AddDays(-1) },
            new CurrencyRate { Id = 4, CurrencyCode = "JPY", CurrencyName = "Японская иена", Rate = 0.58m, Date = DateTime.Now.AddDays(-1) },
            new CurrencyRate { Id = 5, CurrencyCode = "CNY", CurrencyName = "Китайский юань", Rate = 12.30m, Date = DateTime.Now.AddDays(-1) },
            new CurrencyRate { Id = 6, CurrencyCode = "KZT", CurrencyName = "Казахстанский тенге", Rate = 0.20m, Date = DateTime.Now.AddDays(-1) },
            new CurrencyRate { Id = 7, CurrencyCode = "RUB", CurrencyName = "Российский рубль", Rate = 0.95m, Date = DateTime.Now.AddDays(-1) },
            new CurrencyRate { Id = 8, CurrencyCode = "CHF", CurrencyName = "Швейцарский франк", Rate = 101.80m, Date = DateTime.Now.AddDays(-1) },
        };

        private static int _nextId = _currencyRates.Max(r => r.Id) + 1;

        public async Task<CurrencyRatesResponse> GetCurrencyRatesAsync()
        {
            var currencyRates = await _currencyApiClient.GetDetailedRatesAsync();

            return currencyRates;
        }

        public Task<CurrencyRate?> GetCurrencyRateByIdAsync(int id)
        {
            return Task.FromResult(_currencyRates.FirstOrDefault(r => r.Id == id));
        }

        public Task AddCurrencyRateAsync(CurrencyRate rate)
        {
            rate.Id = _nextId++;
            _currencyRates.Add(rate);
            return Task.CompletedTask;
        }

        public Task UpdateCurrencyRateAsync(CurrencyRate rate)
        {
            var existingRate = _currencyRates.FirstOrDefault(r => r.Id == rate.Id);
            if (existingRate != null)
            {
                existingRate.CurrencyCode = rate.CurrencyCode;
                existingRate.CurrencyName = rate.CurrencyName;
                existingRate.Rate = rate.Rate;
                existingRate.Date = rate.Date;
            }
            return Task.CompletedTask;
        }

        public Task DeleteCurrencyRateAsync(int id)
        {
            _currencyRates.RemoveAll(r => r.Id == id);
            return Task.CompletedTask;
        }
    }
}