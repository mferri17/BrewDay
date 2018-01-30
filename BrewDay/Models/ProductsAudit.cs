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
            FinishingProduction = new List<Production>();
            RunningOutStocks = new List<Stock>();
            double daysfornearfp = 3;
            double daysfornearos = 10;
            foreach (Production fp in db.Productions)
            {
                if (fp.DateEndEstimated > DateTime.Now && fp.DateEndEstimated < DateTime.Today.AddDays(daysfornearfp))
                    FinishingProduction.Add(fp);
            }
            foreach (Stock ros in db.Stocks)
            {
                if (ros.ExpireDate > DateTime.Now && ros.ExpireDate < DateTime.Today.AddDays(daysfornearos))
                    RunningOutStocks.Add(ros);
            }

        }

        public List<Production> FinishingProduction { get; set; }
        public List<Stock> RunningOutStocks { get; set; }
    }
}