using System.Net.Http;

namespace AdsSystem.Controllers
{
    public class ControllerBase
    {
        public HttpListenerRequest Request { get; set; }
        public HttpListenerResponse Response { get; set; }
    }
}