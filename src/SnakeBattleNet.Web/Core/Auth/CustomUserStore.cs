using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SnakeBattleNet.Web.Utils;

namespace SnakeBattleNet.Web.Core.Auth
{
    public class CustomUserStore<TUser> : IUserPasswordStore<TUser>, IUserRoleStore<TUser>, IUserSearch<TUser> where TUser : UserIdentity
    {
        private readonly MongoCollection<UserIdentity> _usersCollection;

        public CustomUserStore(MongoCollection<UserIdentity> usersCollection)
        {
            _usersCollection = usersCollection;
        }

        public Task CreateAsync(TUser user)
        {
            return Task.Run(() => _usersCollection.Save(user));
        }

        public Task UpdateAsync(TUser user)
        {
            return Task.Run(() => _usersCollection.Save(user));
        }

        public Task DeleteAsync(TUser user)
        {
            var query = new QueryDocument();
            query.AddRange(new Dictionary<string, object>
            {
                {"_id", user.Id}
            });
            return Task.Run(() => _usersCollection.Remove(query));
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            return Task.Run(() => _usersCollection.FindOneByIdAs<TUser>(userId));
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return Task.Run(() => _usersCollection.AsQueryable<TUser>().SingleOrDefault(x => x.UserName == userName));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return UpdateAsync(user);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.Run(() =>
            {
                var identity = _usersCollection.FindOneById(user.Id);
                return identity == null ? null : identity.PasswordHash;
            });
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.Run(() =>
            {
                var identity = _usersCollection.FindOneById(user.Id);
                return identity != null && identity.PasswordHash.IsNotNullOrEmpty();
            });
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            user.Roles.Add(role);
            return UpdateAsync(user);
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            user.Roles.Remove(role);
            return UpdateAsync(user);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            return Task.Run(() =>
            {
                var identity = _usersCollection.FindOneById(user.Id);
                return identity == null ? new List<string>() : identity.Roles as IList<string>;
            });
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            return Task.Run(() =>
            {
                var identity = _usersCollection.FindOneById(user.Id);
                return identity != null && identity.Roles.Contains(role);
            });
        }

        public void Dispose()
        {
            //_database.Server.Disconnect();
        }

        public Task<IEnumerable<TUser>> FindUsersAsync(string userNamePart, int skip, int take)
        {
            return Task.Run(() => _usersCollection.AsQueryable<TUser>()
                .Where(x => x.UserName.ToLower().Contains(userNamePart.ToLower()))
                .Skip(skip)
                .Take(take)
                .AsEnumerable());
        }
    }
}