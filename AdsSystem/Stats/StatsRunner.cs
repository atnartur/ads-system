using System;
using System.Threading.Tasks;

namespace AdsSystem.Stats
{
    public static class StatsRunner
    {
        public static void Init()
        {
            Console.WriteLine("INIT");
            Task.Run(async () => {
                while(true)
                {
                    new StatsGathering().Run();
                    await Task.Delay(60 * 60 * 1000);
                }
            });
        }
    }
}