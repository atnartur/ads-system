using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdsSystem.Models
{
    public class DayStats
    {
        public int Id { get; set; }
        public int BannerId { get; set; }
        public int ViewsCount { get; set; }
        public int ClicksCount { get; set; }
        public decimal Ctr { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public DayStats() { }

        public DayStats(int bannerId, int viewsCount, int clicksCount, decimal ctr, DateTime date)
        {
            BannerId = bannerId;
            ViewsCount = viewsCount;
            ClicksCount = clicksCount;
            Ctr = ctr;
            Date = date;
        }
    }
}
