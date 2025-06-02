namespace MoneyRate.Data
{
    public class CurrencyApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelaySeconds { get; set; }
    }
}