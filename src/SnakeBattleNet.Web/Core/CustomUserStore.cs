using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SnakeBattleNet.Web.Core
{
    public class IdentityUserLogin
    {
        public virtual string LoginProvider { get; set; }

        public virtual string ProviderKey { get; set; }

        public virtual string UserId { get; set; }

        public virtual CustomIdentityUser User { get; set; }
    }
    public class IdentityRole : IRole
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IdentityRole()
            : this("")
        {
        }

        public IdentityRole(string roleName)
        {
            Id = Guid.NewGuid().ToString();
            Name = roleName;
        }
    }
    public class IdentityUserRole
    {
        public virtual string UserId { get; set; }

        public virtual string RoleId { get; set; }

        public virtual IdentityRole Role { get; set; }

        public virtual CustomIdentityUser User { get; set; }
    }
    public class CustomIdentityUser : IUser
    {
        public virtual string Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual ICollection<IdentityUserLogin> Logins { get; private set; }
        public virtual ICollection<IdentityUserRole> Roles { get; private set; }

        public CustomIdentityUser()
        {
            Id = Guid.NewGuid().ToString();
            Roles = new List<IdentityUserRole>();
            Logins = new List<IdentityUserLogin>();
        }

        public CustomIdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }
    }

    public class CustomUserStore<TUser> : IUserLoginStore<TUser>, IUserRoleStore<TUser>, IUserPasswordStore<TUser> where TUser : CustomIdentityUser
    {
        public void Dispose()
        {
        }

        public Task CreateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            throw new NotImplementedException();
        }
    }
}