using System.Web.Mvc;
using SnakeBattleNet.Persistance;

namespace SnakeBattleNet.Web.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult GetTexture(string id)
        {
            var mongo = new MongoGateway();
            var file = mongo.ReadFile(id);
            return new FileStreamResult(file, "image/bmp");
        }
    }
}
