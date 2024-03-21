using Application.Repository.Interface;
using Domain.Models.Entites;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repo
{
    public class CurrencyRatesRepository : BasicRepository<CurrencyRates>, ICurrencyRatesRepository
    {
        public CurrencyRatesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CurrencyRates> GetBySourceAndTargetAsync(string source, string target)
        {
            return await _context.CurrencyRates
                .AsNoTracking()
                .Where(x => x.SourceCurrencyCode == source && x.TargetCurrencyCode == target)
                .FirstOrDefaultAsync();
        }
        new public async Task<List<CurrencyRates>> GetAllAsync()
        {
            return await _context.CurrencyRates
               .AsNoTracking()
               .OrderBy(x => x.SourceCurrencyCode)
               .ToListAsync();
        }

        public async Task<CurrencyRates> AddFromCsv(string[] values)
        {
            var transaction = new CurrencyRates
            {
                SourceCurrencyCode = values[0],
                TargetCurrencyCode = values[1],
                SourceToTargetRate = decimal.Parse(values[2]),
                TargetToSourceRate = decimal.Parse(values[3]),
                Date = DateTime.UtcNow,
                Provider = "FromCsv"
            };
            return transaction;
        }

        public async Task<CurrencyRates> AddFromXlsx(List<string>values)
        {
            var transaction = new CurrencyRates
            {
                SourceCurrencyCode = values[0],
                TargetCurrencyCode = values[1],
                SourceToTargetRate = decimal.Parse(values[2]),
                TargetToSourceRate = decimal.Parse(values[3]),
                Date = DateTime.UtcNow,
                Provider = "FromXlsx"
            };
            return transaction;
        }
    }
}
