using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SnakeBattleNet.Persistance;

namespace SnakeBattleNet.Mvc.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult GetTexture(string id)
        {
            var file = MongoFileStorage<string>.GetFile(id);
            return new FileStreamResult(file, "image/bmp");
        }

        [Obsolete]
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
