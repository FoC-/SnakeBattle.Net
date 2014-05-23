using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Core.Auth;
using SnakeBattleNet.Web.Utils;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserSearch<UserIdentity> _userSearch;
        private readonly IUserStore<UserIdentity> _userStore;
        private readonly IUserRoleStore<UserIdentity> _roleStore;

        public AdminController(IUserSearch<UserIdentity> userSearch, IUserStore<UserIdentity> userStore, IUserRoleStore<UserIdentity> roleStore)
        {
            _userSearch = userSearch;
            _userStore = userStore;
            _roleStore = roleStore;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListUsers(string username, int skip = 0, int take = 10)
        {
            username = username ?? string.Empty;
            var model = await _userSearch.FindUsersAsync(username, skip, take);
            return View(model);
        }

        public async Task<ActionResult> Manage(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return RedirectToAction("Index");
            }
            var model = await _userStore.FindByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var identity = await _userStore.FindByIdAsync(id);
            await _userStore.DeleteAsync(identity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Roles(IEnumerable<string> roles, string id)
        {
            var identity = await _userStore.FindByIdAsync(id);
            var userRoles = await _roleStore.GetRolesAsync(identity);

            foreach (var role in userRoles)
            {
                await _roleStore.RemoveFromRoleAsync(identity, role);
            }

            foreach (var role in roles)
            {
                await _roleStore.AddToRoleAsync(identity, role);
            }

            return RedirectToAction("Manage", new { id = id });
        }
    }
}