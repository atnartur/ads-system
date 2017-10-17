using System;
using System.Data.Entity;
using AdsSystem.Db;

namespace AdsSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, MigrationConfiguration>());
            
            var listener = new System.Net.Http.HttpListener(System.Net.IPAddress.Parse("0.0.0.0"), 8081);
            try
            {
                listener.Request += (sender, context) => Router.Dispatch(context.Request, context.Response);
                listener.Start();
                Console.ReadKey();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
//            finally
//            {
//                listener.Close();
//            }        
        }
    }
}