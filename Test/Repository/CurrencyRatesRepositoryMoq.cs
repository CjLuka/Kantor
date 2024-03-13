using Application.Repository.Interface;
using Domain.Models.Entites;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Data;

namespace Test.Repository
{
    public class CurrencyRatesRepositoryMoq
    {
        public static Mock<ICurrencyRatesRepository> getCurrencyRatesRepository()
        {
            var _context = new MoqContext();

            var _currencyRatesRepository = new Mock<ICurrencyRatesRepository>();

            _currencyRatesRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(() =>
            {
                return _context.CurrencyRates;
            });

            _currencyRatesRepository.Setup(repo => repo.GetBySourceAndTargetAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string source, string target) =>
            {
                return _context.CurrencyRates
                    .Where(rate => rate.SourceCurrencyCode == source && rate.TargetCurrencyCode == target)
                    .FirstOrDefault();
            });

            _currencyRatesRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return _context.CurrencyRates
                    .Where(rate => rate.Id == id)
                    .FirstOrDefault();
            });

            return _currencyRatesRepository;
        }
    }
}