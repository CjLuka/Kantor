using Application.Repository.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Data;

namespace Test.Repository
{
    public class CurrencyTypesRepositoryMoq
    {
        public static Mock<ICurrencyTypesRepository> getCurrencyTypesRepository()
        {
            var _context = new MoqContext();

            var _currencyTypesRepository = new Mock<ICurrencyTypesRepository>();

            _currencyTypesRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(() => {
                return _context.CurrencyTypes;
            });



            return _currencyTypesRepository;
        }
    }
}
