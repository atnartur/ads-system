using System;
using System.Diagnostics;
using System.Linq;
using AdsSystem.Models;

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
//                DayStat(db);
            }
            sw.Stop();
            Console.WriteLine("Stat gathering (" + startTime + ") finished on " + sw.Elapsed.TotalSeconds + " seconds");
        }

        void BannerStat(Db db)
        {
            Console.WriteLine("Stat gathering: banners");

            var views = db.Views;

            foreach (var banner in db.Banners)
            {
                Console.WriteLine("Stat gathering: banners - " + banner.Id);
                banner.ViewsCount += views.Count(x => x.Id > banner.LastView && x.BannerId == banner.Id);
                banner.ClicksCount += views.Count(x => x.Id > banner.LastView && x.IsClicked && x.BannerId == banner.Id);
                banner.Ctr = Ctr(banner.ViewsCount, banner.ClicksCount);
                banner.LastView = views.OrderByDescending(x => x.Id).First().Id;
                db.Attach(banner);
            }

            db.SaveChanges();
        }

        double Ctr(double views, double clicks) => views > 0 ? clicks / views * 100 : 0;
        
        void DayStat(Db db)
        {
            Console.WriteLine("Stat gathering: days");
            var isHaveDayStats = db.DayStats.FirstOrDefault() != null; // есть ли у нас статистика по дням

            // когда коллекция пустая, не получается достать первый элемент.
            // Значит у нас просто нет баннеров в этой таблице еще
            DayStats lastDay = db.DayStats.OrderByDescending(x => x.Id).FirstOrDefault();
            
            // если статистика за сегодня уже собиралась
            if (isHaveDayStats && lastDay != null && lastDay.Date == DateTime.Now.Date)
            {
                var views = db.Views.Where(x => x.Time.Date == db.DayStats.Last().Date);

                var lastDayCollection = db.DayStats.Where(x => x.Date.Date == DateTime.Now.Date);

                views
                    .GroupBy(x => x.BannerId)
                    .Select(x => new {
                        BannerId = x.Key,
                        ViewsCount = x.Count(),
                        ClicksCount = x.Count(y => y.IsClicked)
                    })
                    .ToList()
                    .ForEach(x =>
                    {
                        Console.WriteLine("Stat gathering: days - " + x.BannerId);
                        lastDay = lastDayCollection.FirstOrDefault(y => y.BannerId == x.BannerId);
                        lastDay.ViewsCount = x.ViewsCount;
                        lastDay.ClicksCount = x.ClicksCount;
                        lastDay.Ctr = Ctr(lastDay.ViewsCount, lastDay.ClicksCount);
                        db.Attach(lastDay);
                    });
            }
            else // если нет, считаем всё
            { 
                var views = isHaveDayStats ? db.Views.Where(x => x.Time > db.DayStats.Last().Date) : db.Views;

                views
                    .GroupBy(x => new { x.Time.Day, x.Time.Month, x.Time.Year, x.BannerId })
                    .Select(x => new {
                        x.First().Time.Date,
                        x.Key.BannerId,
                        ViewsCount = x.Count(),
                        ClicksCount = x.Count(y => y.IsClicked)
                    })
                    .ToList()
                    .ForEach(x =>
                    {
                        Console.WriteLine("Stat gathering: days - " + x.BannerId);
                        
                        db.DayStats.Add(new DayStats
                        {
                            BannerId = x.BannerId,
                            ViewsCount = x.ViewsCount,
                            ClicksCount = x.ClicksCount,
                            Ctr = Ctr(x.ViewsCount, x.ClicksCount),
                            Date = x.Date
                        });
                    });
            }

            db.SaveChanges();
        }
    }
}
