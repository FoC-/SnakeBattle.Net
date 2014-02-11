using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Snake;
using SnakeBattleNet.Core.Snake.Implementation;
using SnakeBattleNet.Persistance;
using SnakeBattleNet.Web.Models;
using Size = SnakeBattleNet.Core.Common.Size;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize]
    public class EditController : Controller
    {
        private IMongoGateway mongoGateway;
        private string CurrentUserId
        {
            get
            {
                MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                return currentUser == null ? null : currentUser.ProviderUserKey.ToString();
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            mongoGateway = new MongoGateway();
            base.Initialize(requestContext);
        }

        public ActionResult Index(string snakeId)
        {
            var snake = mongoGateway.GetById(snakeId);

            if (snake == null || !IsOwner(snake))
                return RedirectToAction("Index", "Training");

            var snakeBrainModules = new SnakeBrainModulesVieModel(snake.Id, snake.SnakeName)
            {
                Ids = snake.BrainModules.Select(module => module.Id),
                Maximum = snake.ModulesMax
            };

            return View(snakeBrainModules);
        }

        public ActionResult EditName(string snakeId)
        {
            ISnake snake = mongoGateway.GetById(snakeId);
            var nameViewModel = new SnakeViewModelBase(snake.Id, snake.SnakeName);

            return View("EditName", nameViewModel);
        }

        [HttpPost]
        public ActionResult EditName(SnakeViewModelBase model)
        {
            if (!ModelState.IsValid)
                return View(model);

            ISnake snake = mongoGateway.GetById(model.Id);

            if (!IsOwner(snake))
                return RedirectToAction("Index", "Training");

            snake.SetName(model.Name);
            mongoGateway.UpdateSnake(snake);

            return RedirectToAction("Index", new { snakeId = model.Id });
        }

        public ActionResult UploadTexture(string snakeId)
        {
            var snake = GetSnakeById(snakeId);
            var snakeViewModelBase = new SnakeViewModelBase(snake.Id, snake.SnakeName);
            return View(snakeViewModelBase);
        }

        [HttpPost]
        public ActionResult UploadTexture(string id, HttpPostedFileBase file)
        {
            if (file != null)
            {
                Image image = null;
                try
                {
                    image = Image.FromStream(file.InputStream);
                }
                catch
                {
                    ModelState.AddModelError("", "Content file data is not image");
                }
                if (image != null && image.Height != 10)
                {
                    ModelState.AddModelError("", "ImageHeight should be 10 px");
                }
                if (image != null && image.Width != 30)
                {
                    ModelState.AddModelError("", "Image Width should be 30 px");
                }
                if (file.ContentLength > 4 * 1024)
                {
                    ModelState.AddModelError("", "File Size should be less then 4 kb");
                }

                if (!ModelState.IsValid)
                {
                    var snake = GetSnakeById(id);
                    var snakeViewModelBase = new SnakeViewModelBase(snake.Id, snake.SnakeName);
                    return View(snakeViewModelBase);
                }

                file.InputStream.Position = 0;
                this.mongoGateway.SaveFile(id, file.FileName, file.InputStream, file.ContentType);
            }
            return RedirectToAction("Index", new { snakeId = id });
        }

        [HttpPost]
        public ActionResult InsertModule(string snakeId, string moduleId, int position)
        {
            var objects = new Dictionary<string, object> { { "Status", "OK" } };

            var size = new Size(7, 7);
            var module = new BrainModule(moduleId, size, snakeId);

            try
            {
                var snake = GetSnakeById(snakeId);
                snake.InsertModule(position, module);
                mongoGateway.UpdateSnake(snake);
            }
            catch (Exception exception)
            {
                objects["Status"] = "Error";
                objects.Add("Error", exception.Message);
            }
            objects.Add("Module", module);

            return Json(objects);
        }

        [HttpPost]
        public ActionResult DeleteModule(string snakeId, string moduleId)
        {
            var objects = new Dictionary<string, object> { { "Status", "OK" } };

            try
            {
                var snake = GetSnakeById(snakeId);
                snake.DeleteModule(moduleId);
                mongoGateway.UpdateSnake(snake);
            }
            catch (Exception exception)
            {
                objects["Status"] = "Error";
                objects.Add("Error", exception.Message);
            }

            return Json(objects);
        }

        [HttpPost]
        public ActionResult GetModule(string snakeId, string moduleId)
        {
            var objects = new Dictionary<string, object> { { "Status", "OK" } };

            IBrainModule module = null;
            try
            {
                var snake = GetSnakeById(snakeId);
                module = snake.BrainModules.Single(m => m.Id == moduleId);
            }
            catch (Exception exception)
            {
                objects["Status"] = "Error";
                objects.Add("Error", exception.Message);
            }

            objects.Add("Module", module);
            return Json(objects);
        }

        public ActionResult EditModule(string snakeId)
        {
            var snake = GetSnakeById(snakeId);
            return View();
        }


        private bool IsOwner(ISnake snake)
        {
            if (snake == null || snake.OwnerId != CurrentUserId)
            {
                ModelState.AddModelError("", "This warrior is not belong to you");
                return false;
            }
            return true;
        }

        private ISnake GetSnakeById(string snakeId)
        {
            return this.mongoGateway.GetById(snakeId);
        }
    }
}
