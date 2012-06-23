using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace SnakeBattleNet.Web.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditUserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string[] UserRoles { get; set; }

        public EditUserModel()
        { }

        public EditUserModel(string username, string email, string[] roles, string[] userRoles)
        {
            this.Username = username;
            this.Email = email;
            this.Roles = roles;
            this.UserRoles = userRoles;
        }
    }

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);

        MembershipUserCollection GetAllUsers();

        MembershipUser GetUser(string username);

        string[] GetAllRoles();

        string[] GetRolesForUser(string username);

        void AddRole(string roleName);

        void UpdateUser(MembershipUser user, string[] roles);

        void DeleteRole(string roleName);
    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;
        private readonly RoleProvider _roleProvider;

        public AccountMembershipService()
            : this(null, null)
        {
        }

        public AccountMembershipService(MembershipProvider provider, RoleProvider roleProvider)
        {
            this._provider = provider ?? Membership.Provider;
            this._roleProvider = roleProvider ?? Roles.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return this._provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            return this._provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            this._provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = this._provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public MembershipUserCollection GetAllUsers()
        {
            int totalRecords;
            return this._provider.GetAllUsers(0, 1000, out totalRecords);
        }

        public MembershipUser GetUser(string username)
        {
            return this._provider.GetUser(username, false);
        }

        public string[] GetAllRoles()
        {
            return this._roleProvider.GetAllRoles();
        }

        public string[] GetRolesForUser(string username)
        {
            return this._roleProvider.GetRolesForUser(username);
        }

        public void AddRole(string roleName)
        {
            this._roleProvider.CreateRole(roleName);
        }

        public void UpdateUser(MembershipUser user, string[] roles)
        {
            this._provider.UpdateUser(user);
            var existingRoles = this._roleProvider.GetRolesForUser(user.UserName);
            if (roles != null && roles.Length > 0)
            {
                var rolesToBeAdded = roles.Except(existingRoles).ToArray();
                this._roleProvider.AddUsersToRoles(new[] { user.UserName }, rolesToBeAdded);
            }
            if (existingRoles.Length > 0)
            {
                var rolesToBeDeleted = (roles != null ? existingRoles.Except(roles) : existingRoles).ToArray();
                this._roleProvider.RemoveUsersFromRoles(new[] { user.UserName }, rolesToBeDeleted);
            }
        }

        public void DeleteRole(string roleName)
        {
            this._roleProvider.DeleteRole(roleName, false);
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion
}
