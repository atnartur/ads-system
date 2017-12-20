using System;
using System.Threading.Tasks;

namespace AdsSystem.Stats
{
    public static class StatsRunner
    {
        public static void Init()
        {
            Console.WriteLine("INIT");
            Task.Run(() => new StatsGathering().Run());
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);
            var t = new System.Threading.Timer(e => new StatsGathering().Run(), null, startTimeSpan, periodTimeSpan);
        }
    }
}