using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyTypes.Queries.GetAll
{
    public class GetAllCurrencyTypesHandler : IRequestHandler<GetAllCurrencyTypesQuery, BaseResponse<List<GetAllCurrencyTypesDto>>>
    {
        private readonly ICurrencyTypesRepository _currencyTypesRepository;
        private readonly IMapper _mapper;
        public GetAllCurrencyTypesHandler(ICurrencyTypesRepository currencyTypesRepository, IMapper mapper)
        {
            _currencyTypesRepository = currencyTypesRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse<List<GetAllCurrencyTypesDto>>> Handle(GetAllCurrencyTypesQuery request, CancellationToken cancellationToken)
        {
            var allCurrencyTypes = await _currencyTypesRepository.GetAllAsync();
            if(allCurrencyTypes.Count == 0)
            {
                return new BaseResponse<List<GetAllCurrencyTypesDto>>(false, "Brak danych");
            }

            return new BaseResponse<List<GetAllCurrencyTypesDto>>(_mapper.Map<List<GetAllCurrencyTypesDto>>(allCurrencyTypes), true);
        }
    }
}
