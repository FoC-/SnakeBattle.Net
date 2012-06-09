using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SnakeBattleNet.Mvc.Controllers
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
                                   {"snakes", new[] {"Content/snake1.bmp", "Content/snake2.bmp", "Content/snake3.bmp", "Content/snake4.bmp"}}
                               };
            return Json(textures);
        }
    }
}
