using System.Web.Mvc;
using EmitMapper;
using SnakeBattleNet.Core.Profile;
using SnakeBattleNet.FE.Filters;
using SnakeBattleNet.FE.Models;
using SnakeBattleNet.Persistance;

namespace SnakeBattleNet.FE.Controllers
{
    public class ProfileController : Controller
    {
        private string UserId { get { return User.Identity.Name; } }
        private readonly IMongoGateway _mongoGateway;

        public ProfileController(IMongoGateway mongoGateway)
        {
            _mongoGateway = mongoGateway;
        }

        public ActionResult Index()
        {
            var model = GetCurrentProfileViewModel();
            return model == null ? (ActionResult)RedirectToAction("Edit") : View(model);
        }

        public ActionResult Edit()
        {
            var model = GetCurrentProfileViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = ObjectMapperManager.DefaultInstance.GetMapper<ProfileViewModel, User>().Map(viewModel, new User(UserId));
                _mongoGateway.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [ChildActionOnly]
        [Anonym]
        public ActionResult LoginMenu()
        {
            var model = GetCurrentProfileViewModel();
            return PartialView("_LoginMenu", model);
        }

        private ProfileViewModel GetCurrentProfileViewModel()
        {
            var profile = _mongoGateway.GetUserById(UserId);
            return ObjectMapperManager.DefaultInstance.GetMapper<IUser, ProfileViewModel>().Map(profile);
        }
    }
}
