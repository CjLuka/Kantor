using Application.Function.CurrencyExchangeTransaction.Queries.GetAll;
using Application.Function.CurrencyRates.Queries.GetAllRates;
using Application.Function.CurrencyRates.Queries.GetBySourceAndTargetAsync;
using Application.Function.CurrencyRates.Queries.GetExchangeRatesFromApi;
using Application.Function.CurrencyTypes.Commands.Add;
using Application.Function.CurrencyTypes.Queries.GetAll;
using AutoMapper;
using Domain.Models.Entites;
using Domain.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AutoMapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            //CurrencyRates
            CreateMap<GetExchangeRatesFromApiDto, CurrencyRates>().ReverseMap();
            
            CreateMap<UpdateCurrencyRatesViewModel, CurrencyRates>().ReverseMap();

            CreateMap<GetAllRatesDto, CurrencyRates>().ReverseMap();

            CreateMap<GetBySourceAndTargetAsyncDto, CurrencyRates>().ReverseMap();

            //CurrencyTypes
            CreateMap<GetAllCurrencyTypesDto, CurrencyTypes>().ReverseMap();
            CreateMap<AddCurrencyTypesCommand, CurrencyTypes>().ReverseMap();

            //CurrencyExchangeTransaction

            CreateMap<GetAllCurrencyExchangeTransactionDto, CurrencyExchangeTransaction>().ReverseMap();
        }
    }
}
