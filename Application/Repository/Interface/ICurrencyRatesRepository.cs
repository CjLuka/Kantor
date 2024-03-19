using Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Repository.Interface
{
    public interface ICurrencyRatesRepository : IBasicRepository<CurrencyRates>
    {
        Task<CurrencyRates> GetBySourceAndTargetAsync(string  source, string target);
        Task<CurrencyRates> AddFromCsv(string[] values);
        Task<CurrencyRates> AddFromXlsx(List<string> values);
    }
}
