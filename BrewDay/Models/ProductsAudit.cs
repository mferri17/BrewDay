using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BrewDay.Domain.Entities;

namespace BrewDay.DTO
{
    public class ProductsAudit
    {
        public ProductsAudit()
        {
            FinishingProduction = new List<Production>();
            RunningOutStocks = new List<Stock>();
        }

        public List<Production> FinishingProduction { get; set; }
        public List<Stock> RunningOutStocks { get; set; }
    }
}