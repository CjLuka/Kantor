using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Commands.ImportFromCsv
{
    public class ImportFromCsvCurrencyRatesCommand : IRequest<BaseResponse>
    {
        public IFormFile FileStream { get; set; }
    }
}
