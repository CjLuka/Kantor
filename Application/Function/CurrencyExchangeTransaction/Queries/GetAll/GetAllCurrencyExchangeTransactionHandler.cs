using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyExchangeTransaction.Queries.GetAll
{
    
    public class GetAllCurrencyExchangeTransactionHandler : IRequestHandler<GetAllCurrencyExchangeTransactionQuery, BaseResponse<List<GetAllCurrencyExchangeTransactionDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrencyExchangeTransactionRepository _currencyExchangeTransactionRepository;
        public GetAllCurrencyExchangeTransactionHandler(IMapper mapper, ICurrencyExchangeTransactionRepository currencyExchangeTransactionRepository)
        {
            _currencyExchangeTransactionRepository = currencyExchangeTransactionRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse<List<GetAllCurrencyExchangeTransactionDto>>> Handle(GetAllCurrencyExchangeTransactionQuery request, CancellationToken cancellationToken)
        {
            var all = await _currencyExchangeTransactionRepository.GetAllAsync();
            if (all.Count == 0)
            {
                return new BaseResponse<List<GetAllCurrencyExchangeTransactionDto>>(false, "Brak danych");
            }
            return new BaseResponse<List<GetAllCurrencyExchangeTransactionDto>>(_mapper.Map<List<GetAllCurrencyExchangeTransactionDto>>(all), true);
        }
    }
}
