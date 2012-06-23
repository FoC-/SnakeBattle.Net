using System;
using System.Web.Mvc;
using System.Web.Routing;
using SnakeBattleNet.Web.Models;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IMembershipService MembershipService { get; set; }
        public IFormsAuthenticationService FormsService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (this.FormsService == null) { this.FormsService = new FormsAuthenticationService(); }
            if (this.MembershipService == null) { this.MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public ActionResult ManageUsers()
        {
            var users = MembershipService.GetAllUsers();
            return View(users);
        }

        public ActionResult ManageRoles()
        {
            var roles = this.MembershipService.GetAllRoles();
            return View(roles);
        }

        [HttpPost]
        public ActionResult ManageRoles(string roleName)
        {
            if (String.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("roleName", "Name is required");
            }
            else
            {
                this.MembershipService.AddRole(roleName);
            }
            return RedirectToAction("ManageRoles");
        }

        public ActionResult EditUser(string username)
        {
            var user = this.MembershipService.GetUser(username);
            var roles = this.MembershipService.GetAllRoles();
            var userRoles = this.MembershipService.GetRolesForUser(user.UserName);

            return View(new EditUserModel(user.UserName, user.Email, roles, userRoles));
        }

        [HttpPost]
        public ActionResult EditUser(EditUserModel model)
        {
            var user = this.MembershipService.GetUser(model.Username);
            this.MembershipService.UpdateUser(user, model.UserRoles);
            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public ActionResult DeleteRole(string roleName)
        {
            this.MembershipService.DeleteRole(roleName);
            return RedirectToAction("ManageRoles");
        }

    }
}
