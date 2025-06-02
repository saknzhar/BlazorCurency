using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MoneyRate.Interfaces.Dtos; 

namespace MoneyRate.Interfaces
{
    public interface ICurrencyApiClient
    {
        Task<Dictionary<string, decimal>> GetAllRatesAsync(string baseCurrency = "USD", CancellationToken cancellationToken = default);
        Task<Dictionary<string, decimal>> GetSpecificRatesAsync(
            IEnumerable<string> currencyCodes,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default);
        Task<decimal?> GetSingleRateAsync(string currencyCode, string baseCurrency = "USD", CancellationToken cancellationToken = default);
        Task<decimal?> GetHistoricalRateAsync(
            string currencyCode,
            DateTime date,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default);
        Task<bool> IsApiAvailableAsync(CancellationToken cancellationToken = default);
        Task<CurrencyRatesResponse> GetDetailedRatesAsync(
            IEnumerable<string>? currencyCodes = null,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default);
        Task<SupportedCodesResponse> GetSupportedCodes(CancellationToken cancellationToken = default);
    }
}