using Demo_ASP_MVC_Modele.BLL.Interfaces;
using Demo_ASP_MVC_Modele.WebApp.Models;
using Demo_ASP_MVC_Modele.WebApp.Models.Mappers;
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
            return View(_service.GetAll().ToViewModel());
        }

        public IActionResult Add()
        {
            return View(new GameForm());
        }

        [HttpPost]
        public IActionResult Add([FromForm] GameForm gameForm)
        {
            if (gameForm.NbPlayerMin > gameForm.NbPlayerMax)
            {
                ModelState.AddModelError("NbPlayerMax", "Le nombre de joueur Maximum doit être superieur ou égale au nombre de joueur minmum");
            }

            if (!ModelState.IsValid)
            {
                return View(gameForm);
            }

            int id = _service.Insert(gameForm.ToModel());

            return RedirectToAction(nameof(Details), new { Id = id });
        }

        public IActionResult Details([FromRoute] int id)
        {
            try
            {
                Game game = _service.GetById(id).ToViewModel();
                return View(game);
            }
            catch (ArgumentNullException e)
            {
                return NotFound();
            }
        }

        public IActionResult Delete([FromRoute] int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
