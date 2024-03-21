using Application.Response;
using Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Commands.ImportFromXlsx
{
    public class ImportFromXlsxCurrencyRatesHandler : IRequestHandler<ImportFromXlsxCurrencyRatesCommand, BaseResponse>
    {
        private readonly CurrencyRatesFileImport _currencyRatesFileImport;
        public ImportFromXlsxCurrencyRatesHandler(CurrencyRatesFileImport currencyRatesFileImport)
        {
            _currencyRatesFileImport = currencyRatesFileImport;
        }
        public async Task<BaseResponse> Handle(ImportFromXlsxCurrencyRatesCommand request, CancellationToken cancellationToken)
        {
            var import = await _currencyRatesFileImport.ImportXlsxAsync(request.FileStream);
            if (import.Success == true)
            {
                return new BaseResponse(true, "Zaimportowano dane z pliku xlsx");
            }
            else
            {
                return new BaseResponse(false, "Coś poszło nie tak");
            }
            
        }
    }
}
