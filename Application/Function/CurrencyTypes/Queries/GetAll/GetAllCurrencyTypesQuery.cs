using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyTypes.Queries.GetAll
{
    public class GetAllCurrencyTypesQuery :IRequest<BaseResponse<List<GetAllCurrencyTypesDto>>>
    {

    }
}
