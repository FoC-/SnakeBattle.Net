using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SnakeBattleNet.MVC.Controllers
{
    public class ManagerController : Controller
    {
        [HttpPost]
        public ActionResult UploadTexture(HttpPostedFileBase file, string ownerId)
        {
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult GetSnakeTexture(string textureId)
        {
            var textures = new Dictionary<string, IEnumerable<string>>
                               {
                                   {"field", new[] {"Content/field.bmp"}},
                                   {"snakes", new[] {"Content/snake.bmp", "Content/snake.bmp"}}
                               };
            return Json(textures);
        }
    }
}
