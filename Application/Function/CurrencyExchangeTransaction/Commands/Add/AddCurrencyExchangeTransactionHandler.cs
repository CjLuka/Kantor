using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyExchangeTransaction.Commands.Add
{
    public class AddCurrencyExchangeTransactionHandler : IRequestHandler<AddCurrencyExchangeTransactionCommand, BaseResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICurrencyExchangeTransactionRepository _currencyExchangeTransactionRepository;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;

        public AddCurrencyExchangeTransactionHandler(IMapper mapper, ICurrencyRatesRepository currencyRatesRepository, ICurrencyExchangeTransactionRepository currencyExchangeTransactionRepository)
        {
            _currencyExchangeTransactionRepository = currencyExchangeTransactionRepository;
            _mapper = mapper;
            _currencyRatesRepository = currencyRatesRepository;
        }
        public async Task<BaseResponse> Handle(AddCurrencyExchangeTransactionCommand request, CancellationToken cancellationToken)
        {
            var currencyRates = await _currencyRatesRepository.GetByIdAsync(request.CurrencyRatesId);
            if (currencyRates == null)
            {
                return new BaseResponse(false, "Brak przeliczników walut o podanym Id");
            }

            var newCurrencyExchange = await _currencyExchangeTransactionRepository.ChangeMoney(request.AmountToConvert, currencyRates);
            await _currencyExchangeTransactionRepository.AddAsync(newCurrencyExchange);

            if(newCurrencyExchange == null)
            {
                return new BaseResponse(false, "Coś poszło nie tak..");
            }
            return new BaseResponse(true, "Zmieniono pieniądze");
        }
    }
}
