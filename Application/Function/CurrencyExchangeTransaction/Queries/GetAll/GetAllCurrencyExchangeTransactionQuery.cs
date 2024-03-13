using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyExchangeTransaction.Queries.GetAll
{
    public class GetAllCurrencyExchangeTransactionQuery : IRequest<BaseResponse<List<GetAllCurrencyExchangeTransactionDto>>>
    {

    }
}
