using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SnakeBattleNet.Web.Core.Auth
{
    public class UserRole : IRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserRole()
            : this("")
        {
        }

        public UserRole(string roleName)
        {
            Id = Guid.NewGuid().ToString();
            Name = roleName;
        }
    }
    public class UserIdentity : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<UserRole> Roles { get; private set; }

        public UserIdentity()
        {
            Id = Guid.NewGuid().ToString();
            Roles = new List<UserRole>();
        }

        public UserIdentity(string userName)
            : this()
        {
            UserName = userName;
        }
    }

    public class CustomUserStore<TUser> : IUserRoleStore<TUser> where TUser : UserIdentity
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
    }
}