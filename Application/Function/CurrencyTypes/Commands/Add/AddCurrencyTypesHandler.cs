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

namespace Application.Function.CurrencyTypes.Commands.Add
{
    public class AddCurrencyTypesHandler : IRequestHandler<AddCurrencyTypesCommand, BaseResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICurrencyTypesRepository _currencyTypesRepository;
        public AddCurrencyTypesHandler(IMapper mapper, ICurrencyTypesRepository currencyTypesRepository)
        {
            _currencyTypesRepository = currencyTypesRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse> Handle(AddCurrencyTypesCommand request, CancellationToken cancellationToken)
        {
            var currencyTypes = _mapper.Map<Domain.Models.Entites.CurrencyTypes>(request);

            if(currencyTypes == null)
            {
                return new BaseResponse(false, "Błąd podczas dodawania nowego typu waluty");
            }
            if(currencyTypes.Name.Length < 3)
            {
                return new BaseResponse(false, "Nazwa waluty nie może zawierać mniej niż 3 znaki");
            }

            var addCurrencyTypes = await _currencyTypesRepository.AddAsync(currencyTypes);

            return new BaseResponse(true, "Dodano nowy typ waluty");
        }
    }
}
