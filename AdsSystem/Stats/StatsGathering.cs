using System;
using System.Diagnostics;
using System.Linq;
using AdsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AdsSystem.Stats
{
    public class StatsGathering
    {
        public void Run()
        {
            var sw = new Stopwatch();
            sw.Start();
            var startTime = DateTime.Now;
            Console.WriteLine("Stat gathering - " + startTime);
            using (var db = Db.Instance)
            {
                BannerStat(db);
                DayStat(db);
            }
            sw.Stop();
            Console.WriteLine("Stat gathering (" + startTime + ") finished on " + sw.Elapsed.TotalSeconds + " seconds");
        }

        void BannerStat(Db db)
        {
            Console.WriteLine("Stat gathering: banners");
            db.Views
                .GroupBy(x => x.BannerId)
                .Select(x => new { BannerId = x.Key, list = x.ToList() })
                .ToList()
                .ForEach(x =>
                {
                    Console.WriteLine("Stat gathering: banners - " + x.BannerId);
                    var banner = db.Banners.FirstOrDefault(y => y.Id == x.BannerId);
                    banner.ViewsCount = x.list.Count;
                    banner.ClicksCount = x.list.Count(y => y.IsClicked);
                    banner.Ctr = banner.ViewsCount > 0 ? banner.ClicksCount / banner.ViewsCount : 0;
                    db.Attach(banner);
                });
            db.SaveChanges();
        }

        void DayStat(Db db)
        {
            Console.WriteLine("Stat gathering: days");
            var count = db.DayStats.Count();
            if (count != 0 && db.DayStats.Last().Date == DateTime.Now.Date)
            {
                var views = db.Views.Where(x => x.Time.Date == db.DayStats.Last().Date).ToList();

                var lastDayCollection = db.DayStats.Where(x => x.Date.Date == DateTime.Now.Date);
                DayStats lastDay;

                views
                    .GroupBy(x => x.BannerId)
                    .Select(x => new { Banner = x.Key, list = x.ToList() })
                    .ToList()
                    .ForEach(x =>
                    {
                        Console.WriteLine("Stat gathering: days - " + x.Banner);
                        lastDay = lastDayCollection.FirstOrDefault(y => y.BannerId == x.Banner);
                        lastDay.ViewsCount = x.list.Count;
                        lastDay.ClicksCount = x.list.Count(y => y.IsClicked);
                        lastDay.Ctr = lastDay.ViewsCount > 0 ? lastDay.ClicksCount / lastDay.ViewsCount : 0;
                        db.Attach(lastDay);
                    });
            }
            else
            {
                var views = db.Views.ToList();

                if (count != 0)
                    views = views.Where(x => x.Time > db.DayStats.Last().Date).ToList();

                views
                    .GroupBy(x => new { x.Time.Date, x.BannerId })
                    .Select(x => new { TimeAndBanner = x.Key, list = x.ToList() })
                    .ToList()
                    .ForEach(x => db.DayStats.Add(new DayStats()
                    {
                        BannerId = x.TimeAndBanner.BannerId,
                        ViewsCount = x.list.Count,
                        ClicksCount = x.list.Count(y => y.IsClicked),
                        Ctr = x.list.Count(y => y.IsClicked) / x.list.Count,
                        Date = x.TimeAndBanner.Date.Date
                    }));
            }

            db.SaveChanges();
        }
    }
}
