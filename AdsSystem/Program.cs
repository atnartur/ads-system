using System;
using AdsSystem.Models;

namespace AdsSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
//            var dbContext = Db.Context.Instance;
//            
//            dbContext.Users.Add(new User()
//            {
//                Email = "a@a.ru",
//                Name = "atnartur",
//                Pass = "asd"
//            });
//            dbContext.SaveChanges();
            var listener = new System.Net.Http.HttpListener(System.Net.IPAddress.Parse("0.0.0.0"), 8081);
            try
            {
                listener.Request += (sender, context) => Router.Dispatch(context.Request, context.Response);
                listener.Start();
                Console.WriteLine("Server is running");
                Console.ReadKey();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
            finally
            {
                listener.Close();
            }        
        }
    }
}