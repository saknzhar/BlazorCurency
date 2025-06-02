
using MoneyRate.Interfaces;
using MoneyRate.Interfaces.Dtos;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options; 
using MoneyRate.Data; 

namespace MoneyRate.Services
{
    public class ExchangeRateApiClient : ICurrencyApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly CurrencyApiSettings _apiSettings;

        public ExchangeRateApiClient(HttpClient httpClient, IOptions<CurrencyApiSettings> apiSettingsOptions)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettingsOptions.Value;
        }

        private async Task<T> GetApiResponse<T>(string endpoint, CancellationToken cancellationToken) where T : ExchangeRateApiResponse
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint, cancellationToken);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                var apiResponse = JsonSerializer.Deserialize<T>(jsonString);

                if (apiResponse == null)
                {
                    throw new JsonException($"Failed to deserialize API response for endpoint: {endpoint}");
                }

                if (apiResponse.Result == "error")
                {
                    throw new HttpRequestException($"ExchangeRate-API Error: {apiResponse.ErrorType} for endpoint: {endpoint}");
                }

                return apiResponse;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message} - Endpoint: {endpoint}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error: {ex.Message} - Endpoint: {endpoint}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message} - Endpoint: {endpoint}");
                throw;
            }
        }

        public async Task<Dictionary<string, decimal>> GetAllRatesAsync(string baseCurrency = "USD", CancellationToken cancellationToken = default)
        {
            var response = await GetApiResponse<ExchangeRatesResponse>($"{_apiSettings.BaseUrl}{_apiSettings.ApiKey}/latest/{baseCurrency}", cancellationToken);
            return response.ConversionRates
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public async Task<Dictionary<string, decimal>> GetSpecificRatesAsync(
            IEnumerable<string> currencyCodes,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default)
        {
            var allRates = await GetAllRatesAsync(baseCurrency, cancellationToken);

            var specificRates = new Dictionary<string, decimal>();
            foreach (var code in currencyCodes)
            {
                if (allRates.TryGetValue(code, out var rate))
                {
                    specificRates.Add(code, rate);
                }
            }
            return specificRates;
        }

        public async Task<decimal?> GetSingleRateAsync(string currencyCode, string baseCurrency = "USD", CancellationToken cancellationToken = default)
        {
            var response = await GetApiResponse<ExchangeRatesResponse>($"{_apiSettings.BaseUrl}{_apiSettings.ApiKey}/latest/{baseCurrency}", cancellationToken);
            if (response.ConversionRates.TryGetValue(currencyCode.ToUpper(), out decimal rate))
            {
                return rate;
            }
            return null;
        }

        public async Task<decimal?> GetHistoricalRateAsync(
            string currencyCode,
            DateTime date,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default)
        {
            var dateString = date.ToString("yyyy/MM/dd");
            var response = await GetApiResponse<ExchangeRatesResponse>($"{_apiSettings.BaseUrl}{_apiSettings.ApiKey}/history/{baseCurrency}/{dateString}", cancellationToken);

            if (response.ConversionRates.TryGetValue(currencyCode.ToUpper(), out decimal rate))
            {
                return rate;
            }
            return null;
        }

        public async Task<bool> IsApiAvailableAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await GetApiResponse<SupportedCodesResponse>($"{_apiSettings.BaseUrl}{_apiSettings.ApiKey}/codes", cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API availability check failed: {ex.Message}");
                return false;
            }
        }

        public async Task<CurrencyRatesResponse> GetDetailedRatesAsync(
            IEnumerable<string>? currencyCodes = null,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default)
        {
            var response = new CurrencyRatesResponse
            {
                BaseCode = baseCurrency,
                Result = "error",
            };

            try
            {
                var latestRatesResponse = await GetApiResponse<ExchangeRatesResponse>($"{_apiSettings.BaseUrl}{_apiSettings.ApiKey}/latest/{baseCurrency}", cancellationToken);

                var supportedCodesResponse = await GetSupportedCodes();

                response.Result = latestRatesResponse.Result;
                response.ErrorType = latestRatesResponse.ErrorType;
                response.TimeLastUpdateUtc = DateTime.Now.ToString();
                response.ConversionRates = latestRatesResponse.ConversionRates;

                response.MergeWithSupportedCodes(supportedCodesResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDetailedRatesAsync: {ex.Message}");
                response.Result = "error";
                response.ErrorType = "client-side-error";
            }

            return response;
        }

        public async Task<SupportedCodesResponse> GetSupportedCodes(CancellationToken cancellationToken = default)
        {
            var response = new SupportedCodesResponse();
            try
            {
                response = await GetApiResponse<SupportedCodesResponse>($"{_apiSettings.BaseUrl}{_apiSettings.ApiKey}/codes", cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API availability check failed: {ex.Message}");
            }

            return response;
        }
    }
}