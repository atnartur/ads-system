using Microsoft.AspNetCore.Http;

namespace AdsSystem.Controllers
{
    public class ControllerBase
    {
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
    }
}