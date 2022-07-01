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
                TempData["ErrorMessage"] = "Le jeu n'a pas été trouvé !!!";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Delete([FromRoute] int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update([FromRoute]int id)
        {
            try
            {
                BLL.Entities.GameModel gameModel = _service.GetById(id);

                GameForm gameForm = new GameForm()
                {
                    Name = gameModel.Name,
                    Age = gameModel.Age,
                    Description = gameModel.Description,
                    NbPlayerMin = gameModel.NbPlayerMin,
                    NbPlayerMax = gameModel.NbPlayerMax,
                    IsCoop = gameModel.IsCoop
                };

                return View(gameForm);
            }
            catch (ArgumentNullException e)
            {
                TempData["ErrorMessage"] = "Le jeu n'a pas été trouvé !!!";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public IActionResult Update([FromRoute] int id, [FromForm]GameForm gameForm)
        {
            if (gameForm.NbPlayerMin > gameForm.NbPlayerMax)
            {
                ModelState.AddModelError("NbPlayerMax", "Le nombre de joueur Maximum doit être superieur ou égale au nombre de joueur minmum");
            }

            if (!ModelState.IsValid)
            {
                return View(gameForm);
            }

            BLL.Entities.GameModel data = gameForm.ToModel();
            data.Id = id;

            bool isOk = _service.Update(data);

            if(isOk)
            {
                return RedirectToAction(nameof(Details), new { Id = id });
            }
            else
            {
                TempData["ErrorMessage"] = "Une erreur est survenu durant la mise à jour !!!";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
