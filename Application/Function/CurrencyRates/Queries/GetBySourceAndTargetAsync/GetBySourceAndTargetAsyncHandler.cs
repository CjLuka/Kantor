using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Queries.GetBySourceAndTargetAsync
{
	public class GetBySourceAndTargetAsyncHandler : IRequestHandler<GetBySourceAndTargetAsyncQuery, BaseResponse<GetBySourceAndTargetAsyncDto>>
	{
		private readonly IMapper _mapper;
		private readonly ICurrencyRatesRepository _currencyRatesRepository;

        public GetBySourceAndTargetAsyncHandler(IMapper mapper, ICurrencyRatesRepository currencyRatesRepository)
        {
            _currencyRatesRepository = currencyRatesRepository;
			_mapper = mapper;
        }
        public async Task<BaseResponse<GetBySourceAndTargetAsyncDto>> Handle(GetBySourceAndTargetAsyncQuery request, CancellationToken cancellationToken)
		{
			var currency = await _currencyRatesRepository.GetBySourceAndTargetAsync(request.Source, request.Target);

			if(currency == null)
			{
				return new BaseResponse<GetBySourceAndTargetAsyncDto>(false, "Brak danych");
			}
			return new BaseResponse<GetBySourceAndTargetAsyncDto>(_mapper.Map<GetBySourceAndTargetAsyncDto>(currency), true);
		}
	}
}
