using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Commands.ImportFromXlsx
{
    public class ImportFromXlsxCurrencyRatesCommand : IRequest<BaseResponse>
    {
        public IFormFile FileStream { get; set; }

    }
}
