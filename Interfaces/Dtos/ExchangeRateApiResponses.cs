// Interfaces/Dtos/ExchangeRateApiResponses.cs
using System.Text.Json.Serialization; // Используем System.Text.Json

namespace MoneyRate.Interfaces.Dtos
{
    // Базовый класс для ответа API
    public class ExchangeRateApiResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; } = string.Empty; // "success", "error"

        [JsonPropertyName("error-type")]
        public string? ErrorType { get; set; } // Например, "api-key-invalid"
    }

    // Ответ для получения всех или определенных курсов (latest, historical)
    public class ExchangeRatesResponse : ExchangeRateApiResponse
    {
        [JsonPropertyName("base_code")]
        public string BaseCode { get; set; } = string.Empty;

        [JsonPropertyName("time_last_update_unix")]
        public long TimeLastUpdateUnix { get; set; }

        [JsonPropertyName("time_last_update_utc")]
        public string TimeLastUpdateUtc { get; set; } = string.Empty;

        [JsonPropertyName("time_next_update_unix")]
        public long TimeNextUpdateUnix { get; set; }

        [JsonPropertyName("time_next_update_utc")]
        public string TimeNextUpdateUtc { get; set; } = string.Empty;

        [JsonPropertyName("conversion_rates")]
        public Dictionary<string, decimal> ConversionRates { get; set; } = new Dictionary<string, decimal>();
    }

    // Ответ для получения списка поддерживаемых валют (codes)
    public class SupportedCodesResponse : ExchangeRateApiResponse
    {
        [JsonPropertyName("supported_codes")]
        public List<List<string>> SupportedCodes { get; set; } = new List<List<string>>(); // [["USD", "United States Dollar"], ...]
    }

    // DTO, который вы запросили для GetDetailedRatesAsync
    public class CurrencyRatesResponse : ExchangeRateApiResponse
    {
        [JsonPropertyName("base_code")]
        public string BaseCode { get; set; } = string.Empty;

        [JsonPropertyName("time_last_update_unix")]
        public long TimeLastUpdateUnix { get; set; }

        [JsonPropertyName("time_last_update_utc")]
        public string TimeLastUpdateUtc { get; set; } = string.Empty;

        [JsonPropertyName("time_next_update_unix")]
        public long TimeNextUpdateUnix { get; set; }

        [JsonPropertyName("time_next_update_utc")]
        public string TimeNextUpdateUtc { get; set; } = string.Empty;

        [JsonPropertyName("conversion_rates")]
        public Dictionary<string, decimal> ConversionRates { get; set; } = new Dictionary<string, decimal>();

        // Additional property for detailed rates with metadata
        public Dictionary<string, CurrencyRateDetail> DetailedRates { get; set; } = new Dictionary<string, CurrencyRateDetail>();

        // Add supported codes from SupportedCodesResponse
        public List<List<string>> SupportedCodes { get; set; } = new List<List<string>>();

        // Helper method to populate DetailedRates from SupportedCodes
        public void PopulateDetailedRatesFromSupportedCodes()
        {
            DetailedRates.Clear();
            foreach (var supportedCode in SupportedCodes)
            {
                if (supportedCode.Count >= 2)
                {
                    var currencyCode = supportedCode[0];
                    var currencyName = supportedCode[1];

                    DetailedRates[currencyCode] = new CurrencyRateDetail
                    {
                        CurrencyCode = currencyCode,
                        CurrencyName = currencyName
                    };
                }
            }
        }

        // Helper method to merge data from SupportedCodesResponse
        public void MergeWithSupportedCodes(SupportedCodesResponse supportedCodesResponse)
        {
            if (supportedCodesResponse?.SupportedCodes != null)
            {
                SupportedCodes = supportedCodesResponse.SupportedCodes;
                PopulateDetailedRatesFromSupportedCodes();
            }
        }

        // Helper property to get flattened list for DataGrid
        [JsonIgnore]
        public List<CurrencyRateDetail> CurrencyRatesList
        {
            get
            {
                var lastUpdateDate = TimeLastUpdateUnix > 0
                    ? DateTimeOffset.FromUnixTimeSeconds(TimeLastUpdateUnix).DateTime
                    : DateTime.Now;

                return ConversionRates.Select((kvp, index) => new CurrencyRateDetail
                {
                    Id = index + 1,
                    CurrencyCode = kvp.Key,
                    CurrencyName = DetailedRates.ContainsKey(kvp.Key)
                        ? DetailedRates[kvp.Key].CurrencyName
                        : kvp.Key, // Fallback to code if name not available
                    Rate = kvp.Value,
                    Date = lastUpdateDate
                }).ToList();
            }
        }
    }

    public class CurrencyRateDetail
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}