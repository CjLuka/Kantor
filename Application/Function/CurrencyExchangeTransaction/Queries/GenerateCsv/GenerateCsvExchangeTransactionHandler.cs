using Application.Repository.Interface;
using Application.Response;
using Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyExchangeTransaction.Queries.GenerateCsv
{
    public class GenerateCsvExchangeTransactionHandler : IRequestHandler<GenerateCsvExchangeTransactionQuery, BaseResponse>
    {
        private readonly FileGeneratorService _fileGeneratorService;

        public GenerateCsvExchangeTransactionHandler(FileGeneratorService fileGeneratorService)
        {
            _fileGeneratorService = fileGeneratorService;
        }
        public async Task<BaseResponse> Handle(GenerateCsvExchangeTransactionQuery request, CancellationToken cancellationToken)
        {
            await _fileGeneratorService.GenerateCsvAsync();

            return new BaseResponse(true, "Wygenerowano plik");
        }
    }
}
