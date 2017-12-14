using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AdsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AdsSystem.Stats
{
    public class StatsGathering
    {
        private Db db = Db.Instance;

        public void Run()
        {
            BannerStat();
            DayStat();
        }

        void BannerStat()
        {
            db.Banners.ToList().ForEach(x =>
            {
                x.ViewsCount = x.Views.Count;
                x.ClicksCount = x.Views.Where(y => y.IsClicked).Count();
                x.Ctr = x.ClicksCount / x.ViewsCount;
            });
            db.SaveChanges();
        }

        void DayStat()
        {
            var count = db.DayStats.Count();
            if (count != 0 && db.DayStats.Last().Date == DateTime.Now.Date)
            {
                var views = db.Views.Where(x => x.Time.Date == db.DayStats.Last().Date).ToList();

                var lastDayCollection = db.DayStats.Where(x => x.Date.Date == DateTime.Now.Date);
                DayStats lastDay;

                views
                    .GroupBy(x => x.Banner)
                    .Select(x => new { Banner = x.Key, list = x.ToList() })
                    .ToList()
                    .ForEach(x =>
                    {
                        lastDay = lastDayCollection.FirstOrDefault(y => y.BannerId == x.Banner.Id);
                        lastDay.ViewsCount = x.list.Count;
                        lastDay.ClicksCount = x.list.Count(y => y.IsClicked == true);
                        lastDay.Ctr = x.list.Count(y => y.IsClicked == true) / x.list.Count;
                        db.Entry(lastDay).State = EntityState.Modified;
                    });

            }
            else
            {
                var views = db.Views.ToList();

                if (count != 0)
                    views = views.Where(x => x.Time > db.DayStats.Last().Date).ToList();

                views
                    .GroupBy(x => new { x.Time.Date, x.Banner })
                    .Select(x => new { TimeAndBanner = x.Key, list = x.ToList() })
                    .ToList()
                    .ForEach(x => db.DayStats.Add(new DayStats()
                    {
                        BannerId = x.TimeAndBanner.Banner.Id,
                        ViewsCount = x.list.Count,
                        ClicksCount = x.list.Count(y => y.IsClicked == true),
                        Ctr = x.list.Count(y => y.IsClicked == true) / x.list.Count,
                        Date = x.TimeAndBanner.Date.Date
                    }));
            }
            
            db.SaveChanges();
        }
    }
}
