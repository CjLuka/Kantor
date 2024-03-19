using Application.Response;
using Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyExchangeTransaction.Queries.GenerateXlsx
{
    public class GenerateXlsxExchangeTransactionHandler : IRequestHandler<GenerateXlsxExchangeTransactionQuery, BaseResponse>
    {
        private readonly FileGeneratorService _fileGeneratorService;
        public GenerateXlsxExchangeTransactionHandler(FileGeneratorService fileGeneratorService)
        {
            _fileGeneratorService = fileGeneratorService;
        }
        public async Task<BaseResponse> Handle(GenerateXlsxExchangeTransactionQuery request, CancellationToken cancellationToken)
        {
            await _fileGeneratorService.GenerateXlsxAsync();

            return new BaseResponse(true, "Wygenerowano plik xlsx");
        }
    }
}
