using Application.Repository.Interface;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Repository;

namespace Test
{
    public class BaseTest
    {
        protected readonly Mock<ICurrencyTypesRepository> _currencyTypesRepository;
        protected readonly Mock<ICurrencyRatesRepository> _currencyRatesRepository;        
        protected readonly Mock<ICurrencyExchangeTransactionRepository> _currencyExchangeTransactionRepository;        
        protected readonly IMapper _mapper;

        public BaseTest()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Application.AutoMapper.Mapper>();
            });
            _mapper = configurationProvider.CreateMapper();
            _currencyTypesRepository = CurrencyTypesRepositoryMoq.getCurrencyTypesRepository();
            _currencyRatesRepository = CurrencyRatesRepositoryMoq.getCurrencyRatesRepository();
            _currencyExchangeTransactionRepository = CurrencyExchangeTransactionsRepositoryMoq.getCurrencyExchangeTransactionRepository();
        }
    }
}
