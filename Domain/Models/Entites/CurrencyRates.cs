﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entites
{
    public class CurrencyRates
    {
        public int Id { get; set; }
        public string SourceCurrencyCode { get; set; } 
        public string TargetCurrencyCode { get; set; } 
        public decimal SourceToTargetRate { get; set; } 
        public decimal TargetToSourceRate { get; set; }
        public DateTime Date { get; set; }
        public string Provider { get; set; }
    }
}
