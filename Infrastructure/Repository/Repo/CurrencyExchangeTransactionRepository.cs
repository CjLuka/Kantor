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
    public class CurrencyExchangeTransactionRepository : BasicRepository<CurrencyExchangeTransaction>, ICurrencyExchangeTransactionRepository
    {
        public CurrencyExchangeTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CurrencyExchangeTransaction> ChangeMoney(decimal amountToConvert, CurrencyRates currencyRates)
        {
            CurrencyExchangeTransaction currencyExchangeTransaction = new CurrencyExchangeTransaction
            {
                CurrencyRatesId = currencyRates.Id,
                Date = DateTime.UtcNow,
                SourceAmount = amountToConvert,
                TargetAmount = amountToConvert * currencyRates.TargetToSourceRate
            };
            //await _context.AddAsync(currencyExchangeTransaction);
            //await _context.SaveChangesAsync();
            return currencyExchangeTransaction;
        }
        new public async Task<List<CurrencyExchangeTransaction>> GetAllAsync()
        {
            return await _context.CurrencyExchangeTransactions
                .Include(x => x.CurrencyRates)
                .ToListAsync();
        }
        new public async Task<CurrencyExchangeTransaction> GetByIdAsync(int id)
        {
            return await _context.CurrencyExchangeTransactions
                .Include(x => x.CurrencyRates)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
