using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Queries.GetExchangeRatesFromApi
{
    public class GetExchangeRatesFromApiQuery : IRequest<BaseResponse<List<GetExchangeRatesFromApiDto>>>
    {

    }
}
