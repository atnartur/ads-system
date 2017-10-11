using System;

namespace AdsSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var listener = new System.Net.Http.HttpListener(System.Net.IPAddress.Parse("127.0.0.1"), 8081);
            try 
            {
                listener.Request += (sender, context) => Router.Dispatch(context.Request, context.Response);
                listener.Start();
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
            catch(Exception exc) 
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