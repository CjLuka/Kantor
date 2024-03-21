using Application.Function.CurrencyExchangeTransaction.Queries.GetAll;
using Application.Repository.Interface;
using iText.Layout.Element;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using System.Threading.Tasks;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Serilog;
using Application.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class FileGeneratorService
    {
        private readonly ICurrencyExchangeTransactionRepository _currencyExchangeTransactionRepository;
        public FileGeneratorService(ICurrencyExchangeTransactionRepository currencyExchangeTransactionRepository)
        {
            _currencyExchangeTransactionRepository = currencyExchangeTransactionRepository;
        }
        public FileGeneratorService()
        {

        }
        //Generowanie CSV
        public async Task<BaseResponse<string>> GenerateCsvAsync()
        {
            var allCurrencyExchangeTransaction = await _currencyExchangeTransactionRepository.GetAllAsync();

            var fileName = "ExchangeTransaction " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".csv";

            var binPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            var expectedDirectory = Path.Combine(binPath, "ExchangeTransactionFiles_Csv");

            if (!Directory.Exists(expectedDirectory))
                Directory.CreateDirectory(expectedDirectory);

            var path = Path.Combine(expectedDirectory, fileName);

            var csv = new StringBuilder();

            csv.AppendLine("\"Nazwa waluty bazowej\";\"Nazwa waluty docelowej\";\"Wartość waluty bazowej\";\"Wartość po przeliczeniu\";\"Data ostatniej aktualizacji\";");

            foreach (var item in allCurrencyExchangeTransaction)
            {
                var sourceCode = item.CurrencyRates.SourceCurrencyCode;
                var targetCode = item.CurrencyRates.TargetCurrencyCode;
                var sourceAmount = item.SourceAmount;
                var targetAmount = item.TargetAmount;
                var date = item.Date.ToString("yyyy-MM-dd HH:mm:ss");
                var line = $"{sourceCode};{targetCode};{sourceAmount};{targetAmount};{date}";
                csv.AppendLine(line);
            }

            File.WriteAllText(path, csv.ToString());
            Log.Information("Poprawnie wygenerowano wszystkie transackje w pliku csv " + fileName);
            return new BaseResponse<string>(path, true, "Poprawnie wygenerowano plik csv");
        }

        //Generowanie XLSX
        public async Task<BaseResponse<string>> GenerateXlsxAsync()
        {
            var allCurrencyExchangeTransaction = await _currencyExchangeTransactionRepository.GetAllAsync();

            var fileName = "ExchangeTransaction " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".xlsx";

            var binPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            var expectedDirectory = Path.Combine(binPath, "ExchangeTransactionFiles_Xlsx");

            if (!Directory.Exists(expectedDirectory))
                Directory.CreateDirectory(expectedDirectory);

            var path = Path.Combine(expectedDirectory, fileName);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(path)))
            {

                var worksheet = package.Workbook.Worksheets.Add("Transactions");

                worksheet.Cells[1, 1].Value = "Nazwa waluty bazowej";
                worksheet.Cells[1, 2].Value = "Nazwa waluty docelowej";
                worksheet.Cells[1, 3].Value = "Wartość waluty bazowej";
                worksheet.Cells[1, 4].Value = "Wartość po przeliczeniu";
                worksheet.Cells[1, 5].Value = "Data ostatniej aktualizacji";

                for (int i = 0; i < allCurrencyExchangeTransaction.Count; i++)
                {
                    var conversion = allCurrencyExchangeTransaction[i];
                    worksheet.Cells[i + 2, 1].Value = conversion.CurrencyRates.SourceCurrencyCode;
                    worksheet.Cells[i + 2, 2].Value = conversion.CurrencyRates.TargetCurrencyCode;
                    worksheet.Cells[i + 2, 3].Value = conversion.SourceAmount;
                    worksheet.Cells[i + 2, 4].Value = conversion.TargetAmount;
                    worksheet.Cells[i + 2, 5].Value = conversion.Date.ToString("yyyy-MM-dd HH:mm:ss");
                }

                package.Save();
                Log.Information("Poprawnie wygenerowano wszystkie transackje w pliku xlsx " + fileName);
                return new BaseResponse<string>(path,true, "Poprawnie wygenerowano plik Xlsx");
            }
        }

        //Generowanie PDF
        public async Task<BaseResponse<string>> GeneratePdfAsync()
        {
            var allCurrencyExchangeTransaction = await _currencyExchangeTransactionRepository.GetAllAsync();

            var fileName = "ExchangeTransaction " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".pdf";

            var binPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            var expectedDirectory = Path.Combine(binPath, "ExchangeTransactionFiles_Pdf");

            if (!Directory.Exists(expectedDirectory))
                Directory.CreateDirectory(expectedDirectory);

            var path = Path.Combine(expectedDirectory, fileName);

            using (var pdfWriter = new PdfWriter(path))
            {
                using (var pdfDocument = new PdfDocument(pdfWriter))
                {
                    var document = new Document(pdfDocument);

                    var table = new Table(5);

                    table.AddHeaderCell("Nazwa waluty bazowej");
                    table.AddHeaderCell("Nazwa waluty docelowej");
                    table.AddHeaderCell("Wartość waluty bazowej");
                    table.AddHeaderCell("Wartość po przeliczeniu");
                    table.AddHeaderCell("Data ostatniej aktualizacji");

                    foreach (var conversion in allCurrencyExchangeTransaction)
                    {
                        table.AddCell(conversion.CurrencyRates.SourceCurrencyCode);
                        table.AddCell(conversion.CurrencyRates.TargetCurrencyCode);
                        table.AddCell(conversion.SourceAmount.ToString());
                        table.AddCell(conversion.TargetAmount.ToString());
                        table.AddCell(conversion.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    document.Add(table);
                    Log.Information("Poprawnie wygenerowano wszystkie transackje w pliku pdf " + fileName);
                    return new BaseResponse<string>(path, true, "Poprawnie wygenerowano plik Pdf");
                }
            }
        }

    }
}
