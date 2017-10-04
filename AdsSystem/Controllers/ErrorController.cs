namespace AdsSystem.Controllers
{
    public class ErrorController : ControllerBase
    {
        public string E404()
        {
            Response.StatusCode = 404;
            return "Error 404 - not found";
        }
    }
}