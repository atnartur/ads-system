using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AdsSystem.Stats;

namespace AdsSystem
{
    internal class Program
    {
        class Startup
        {
            public void Configure(IApplicationBuilder app, IHostingEnvironment env) =>
                app.Run(async context => Router.Dispatch(context.Request, context.Response));
        }

        public static void Main(string[] args)
        {
            //            Console.WriteLine(User.PassHash("1234"));

            //new StatsGathering().Run();

            //Task.Run(() =>
            //{
            //    new StatsGathering().Run();
            //});

            using (var instance = Db.Instance)
                instance.Database.Migrate();

            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();

            host.Run();
        }
    }
}