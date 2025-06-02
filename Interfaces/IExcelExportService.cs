using MoneyRate.Interfaces.Dtos;

namespace MoneyRate.Interfaces
{
    public interface IExcelExportService
    {
        Task<byte[]> ExportCurrencyRatesToExcelAsync(List<CurrencyRateDetail> currencyRates, string? fileName = null);

    }
}
