using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SnakeBattleNet.Utils.Extensions;
using SnakeBattleNet.Web.Models;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public ActionResult ManageUsers()
        {
            var users = Membership.GetAllUsers();
            return View(users);
        }

        public ActionResult ManageRoles()
        {
            var roles = Roles.GetAllRoles();
            return View(roles);
        }

        [HttpPost]
        public ActionResult ManageRoles(string roleName)
        {
            if (roleName.IsNullOrEmpty())
            {
                ModelState.AddModelError("roleName", "Name is required");
            }
            else
            {
                Roles.CreateRole(roleName);
            }
            return RedirectToAction("ManageRoles");
        }

        public ActionResult EditUser(string username)
        {
            var user = Membership.GetUser(username);
            var roles = Roles.GetAllRoles();
            var userRoles = Roles.GetRolesForUser(user.UserName);

            return View(new EditUserModel(user.UserName, user.Email, roles, userRoles));
        }

        [HttpPost]
        public ActionResult EditUser(EditUserModel model)
        {
            var user = Membership.GetUser(model.Username);
            user.Email = model.Email;
            Membership.UpdateUser(user);

            var roles = model.UserRoles;
            if (roles != null && roles.Length > 0)
            {
                var rolesToBeAdded = roles.Except(Roles.GetRolesForUser(user.UserName)).ToArray();
                if (rolesToBeAdded.Length != 0)
                    Roles.AddUsersToRoles(new[] { user.UserName }, rolesToBeAdded);
            }
            if (Roles.GetRolesForUser(user.UserName).Length > 0)
            {
                var rolesForUser = Roles.GetRolesForUser(user.UserName);
                var rolesToBeDeleted = roles == null ? rolesForUser : rolesForUser.Except(roles).ToArray();

                if (rolesToBeDeleted.Length != 0)
                    Roles.RemoveUsersFromRoles(new[] { user.UserName }, rolesToBeDeleted);
            }

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public ActionResult DeleteRole(string roleName)
        {
            Roles.DeleteRole(roleName);
            return RedirectToAction("ManageRoles");
        }
    }
}
