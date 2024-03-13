using Application.Repository.Interface;
using Domain.Models.Entites;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repo
{
    public class CurrencyTypesRepository : BasicRepository<CurrencyTypes>, ICurrencyTypesRepository
    {
        public CurrencyTypesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
