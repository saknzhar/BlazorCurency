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

        // Добавим поле для метаданных (чтобы соответствовать вашей идее о детальных курсах)
        // Хотя само API не предоставляет метаданные в таком виде для conversion_rates,
        // мы можем дополнить это информацией из GetSupportedCurrenciesAsync
        public Dictionary<string, CurrencyRateDetail> DetailedRates { get; set; } = new Dictionary<string, CurrencyRateDetail>();
    }

    public class CurrencyRateDetail
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; }
    }
}