using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using Domain.Models.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Queries.GetAllRates
{
    public class GetAllRatesHandler : IRequestHandler<GetAllRatesQuery, BaseResponse<List<GetAllRatesDto>>>
    {
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly IMapper _mapper;
        public GetAllRatesHandler(ICurrencyRatesRepository currencyRatesRepository, IMapper mapper)
        {
            _currencyRatesRepository = currencyRatesRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse<List<GetAllRatesDto>>> Handle(GetAllRatesQuery request, CancellationToken cancellationToken)
        {
            var allRates = await _currencyRatesRepository.GetAllAsync();
            if(allRates.Count == 0)
            {
                return new BaseResponse<List<GetAllRatesDto>>(false, "Brak danych");
            }
            return new BaseResponse<List<GetAllRatesDto>>(_mapper.Map<List<GetAllRatesDto>>(allRates), true, "Test");
        }
    }
}
