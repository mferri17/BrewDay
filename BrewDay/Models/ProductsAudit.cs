using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrewDay.Domain;
using BrewDay.Domain.Entities;

namespace BrewDay.DTO
{
    public class ProductsAudit
    {
        private readonly BrewDayContext db = new BrewDayContext();

        public ProductsAudit()
        {
            var tresholdFinishingProd = DateTime.Now.AddDays(7);
            var tresholdExpireStock = DateTime.Now.AddDays(7);
            double tresholdQtyStock = 10;

            FinishingProduction = db.Productions.Where(x => x.DateEndEstimated < tresholdFinishingProd).ToList();
            RunningOutStocks = db.Stocks.Where(x => x.ExpireDate < tresholdExpireStock || x.Quantity < tresholdQtyStock).ToList();
        }

        public List<Production> FinishingProduction { get; set; }
        public List<Stock> RunningOutStocks { get; set; }
    }
}