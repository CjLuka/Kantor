using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyTypes.Commands.Add
{
    public class AddCurrencyTypesCommand : IRequest<BaseResponse>
    {
        public string Name { get; set; }
    }
}
