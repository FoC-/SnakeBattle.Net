using System.Web.Mvc;
using SnakeBattleNet.Persistance;

namespace SnakeBattleNet.Web.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult GetTexture(string id)
        {
            var mongo = new MongoGateway();
            string contentType;
            var file = mongo.ReadFile(id, out contentType);
            if (file == null)
            {
                return File(Server.MapPath("/img") + "/empty-pixel.png", "image/png");
            }
            return new FileStreamResult(file, contentType);
        }
    }
}