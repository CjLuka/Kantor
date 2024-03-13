using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Queries.GetAllRates
{
    public class GetAllRatesQuery : IRequest<BaseResponse<List<GetAllRatesDto>>>
    {

    }
}
