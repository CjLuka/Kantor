using Application.Response;
using Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Commands.ImportFromCsv
{
    public class ImportFromCsvCurrencyRatesHandler : IRequestHandler<ImportFromCsvCurrencyRatesCommand, BaseResponse>
    {
        private readonly CurrencyRatesFileImport _currencyRatesFileImport;
        public ImportFromCsvCurrencyRatesHandler(CurrencyRatesFileImport currencyRatesFileImport)
        {
            _currencyRatesFileImport = currencyRatesFileImport;
        }
        public async Task<BaseResponse> Handle(ImportFromCsvCurrencyRatesCommand request, CancellationToken cancellationToken)
        {
            var import = await _currencyRatesFileImport.ImportCsvAsync(request.FileStream);
            
            if (import.Success == true)
            {
                return new BaseResponse(true, "Zaimportowano dane walut z pliku csv");
            }
            return new BaseResponse(false, "Coś poszło nie tak..");
        }
    }
}
