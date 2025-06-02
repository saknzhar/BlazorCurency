using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using MoneyRate.Interfaces;
using MoneyRate.Interfaces.Dtos;
using DocumentFormat.OpenXml.Packaging;

namespace MoneyRate.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public async Task<byte[]> ExportCurrencyRatesToExcelAsync(List<CurrencyRateDetail> currencyRates, string? fileName = null)
        {
            // Ensure currencyRates is not null to avoid NullReferenceException
            if (currencyRates == null)
            {
                currencyRates = new List<CurrencyRateDetail>();
            }

            using (MemoryStream stream = new MemoryStream())
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook(); // This line is correct. You create a new Workbook object and assign it to the WorkbookPart.

                    // Add a WorksheetPart to the WorkbookPart
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    // IMPORTANT CORRECTION HERE:
                    // Access the Workbook object through the WorkbookPart to append Sheets
                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets()); // Corrected line
                    Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Currency Rates" };
                    sheets.Append(sheet);

                    // Get the SheetData (where cells are added)
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // --- Add Header Row ---
                    Row headerRow = new Row();
                    headerRow.Append(
                        CreateCell("ID", CellValues.String),
                        CreateCell("Код валюты", CellValues.String),
                        CreateCell("Название валюты", CellValues.String),
                        CreateCell("Курс", CellValues.String),
                        CreateCell("Дата", CellValues.String)
                    );
                    sheetData.Append(headerRow);

                    // --- Add Data Rows ---
                    foreach (var rate in currencyRates)
                    {
                        Row dataRow = new Row();
                        dataRow.Append(
                            CreateCell(rate.Id.ToString(), CellValues.Number),
                            CreateCell(rate.CurrencyCode, CellValues.String),
                            CreateCell(rate.CurrencyName, CellValues.String),
                            CreateCell(rate.Rate.ToString("N4", System.Globalization.CultureInfo.InvariantCulture), CellValues.Number),
                            CreateCell(rate.Date.ToString("yyyy-MM-dd"), CellValues.String)
                        );
                        sheetData.Append(dataRow);
                    }

                    // Save the workbook
                    workbookPart.Workbook.Save(); // This line was already correct
                }

                return stream.ToArray();
            }
        }

        private Cell CreateCell(string text, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(text),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }
    }
}
