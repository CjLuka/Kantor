using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Queries.GetBySourceAndTargetAsync
{
	public class GetBySourceAndTargetAsyncQuery : IRequest<BaseResponse<GetBySourceAndTargetAsyncDto>>
	{
        public string Source { get; set; }
        public string Target { get; set; }
    }
}
