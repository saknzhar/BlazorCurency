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

        /// <summary>
        /// Получить курсы определенных валют
        /// </summary>
        /// <param name="currencyCodes">Коды валют для получения курсов</param>
        /// <param name="baseCurrency">Базовая валюта (по умолчанию USD)</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Словарь с курсами указанных валют</returns>
        Task<Dictionary<string, decimal>> GetSpecificRatesAsync(
            IEnumerable<string> currencyCodes,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить курс конкретной валюты
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="baseCurrency">Базовая валюта (по умолчанию USD)</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Курс валюты или null если не найден</returns>
        Task<decimal?> GetSingleRateAsync(string currencyCode, string baseCurrency = "USD", CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить исторические данные курса валюты
        /// </summary>
        /// <param name="currencyCode">Код валюты</param>
        /// <param name="date">Дата для получения исторического курса</param>
        /// <param name="baseCurrency">Базовая валюта (по умолчанию USD)</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Исторический курс валюты или null если не найден</returns>
        Task<decimal?> GetHistoricalRateAsync(
            string currencyCode,
            DateTime date,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить список поддерживаемых валют
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Словарь с кодами и названиями валют</returns>
        Task<Dictionary<string, string>> GetSupportedCurrenciesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Проверить доступность API
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>True если API доступно, иначе false</returns>
        Task<bool> IsApiAvailableAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить подробную информацию о курсах валют с метаданными
        /// </summary>
        /// <param name="currencyCodes">Коды валют (если null - все валюты)</param>
        /// <param name="baseCurrency">Базовая валюта</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Детальная информация о курсах</returns>
        Task<CurrencyRatesResponse> GetDetailedRatesAsync(
            IEnumerable<string>? currencyCodes = null,
            string baseCurrency = "USD",
            CancellationToken cancellationToken = default);
    }
}