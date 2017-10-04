namespace AdsSystem.Controllers
{
    public class IndexController : ControllerBase
    {   
        public string Index()
        {
            return new View("Index").ToString();
        }
    }
}