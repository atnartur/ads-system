using System.IO;
using AdsSystem.Stats;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

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
            using (var instance = Db.Instance)
                instance.Database.Migrate();

            StatsRunner.Init();
            
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();

            host.Run();
        }
    }
}