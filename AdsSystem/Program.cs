﻿using System;
using System.IO;
using AdsSystem.Models;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace AdsSystem
{
    internal class Program
    {
        class Startup
        {
            public void Configure(IApplicationBuilder app,IHostingEnvironment env) =>
                app.Run(async context => Router.Dispatch(context.Request, context.Response));
        }
        
        public static void Main(string[] args)
        {
//            Console.WriteLine(User.PassHash("1234"));
            
            Handlebars.RegisterHelper("ifEq", (writer, context, parameters) => {
                writer.Write(parameters[0].Equals(parameters[1]) ? context : !context);
            });
            
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