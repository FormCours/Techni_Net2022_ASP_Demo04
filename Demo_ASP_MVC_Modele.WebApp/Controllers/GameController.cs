using Demo_ASP_MVC_Modele.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo_ASP_MVC_Modele.WebApp.Controllers
{
    public class GameController : Controller
    {
        private IGameService _service;

        public GameController(IGameService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View(_service.GetAll());
        }
    }
}
