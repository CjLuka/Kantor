using Application.Response;
using Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyExchangeTransaction.Queries.GeneratePdf
{
    public class GeneratePdfExchangeTransactionHandler : IRequestHandler<GeneratePdfExchangeTransactionQuery, BaseResponse>
    {
        private readonly FileGeneratorService _fileGeneratorService;
        public GeneratePdfExchangeTransactionHandler(FileGeneratorService fileGeneratorService)
        {
            _fileGeneratorService = fileGeneratorService;
        }
        public async Task<BaseResponse> Handle(GeneratePdfExchangeTransactionQuery request, CancellationToken cancellationToken)
        {
            await _fileGeneratorService.GeneratePdfAsync();

            return new BaseResponse(true, "Wygenerowano PDF");
        }
    }
}
