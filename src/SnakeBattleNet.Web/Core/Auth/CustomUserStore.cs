using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SnakeBattleNet.Web.Utils;

namespace SnakeBattleNet.Web.Core.Auth
{
    public class CustomUserStore<TUser> : IUserPasswordStore<TUser>, IUserRoleStore<TUser> where TUser : UserIdentity
    {
        private readonly MongoDatabase _database;
        private static string ConnectionString
        {
            get { return ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ?? "mongodb://localhost/SnakeBattle"; }
        }
        private MongoCollection<UserIdentity> UsersCollection
        {
            get { return _database.GetCollection<UserIdentity>(typeof(UserIdentity).Name); }
        }

        static CustomUserStore()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(UserIdentity)))
            {
                BsonClassMap.RegisterClassMap<UserIdentity>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.SetIsRootClass(true);
                    cm.MapIdField(c => c.Id);
                    cm.MapProperty(c => c.UserName).SetElementName("Username");
                    cm.MapProperty(c => c.PasswordHash).SetElementName("PasswordHash");
                    cm.MapProperty(c => c.Roles).SetElementName("Roles").SetIgnoreIfNull(true);
                });
            }
        }

        public CustomUserStore()
        {
            // FACTORY!?!?!?
            var mongoUrl = new MongoUrl(ConnectionString);
            var server = new MongoClient(mongoUrl).GetServer();
            _database = server.GetDatabase(mongoUrl.DatabaseName);

            UsersCollection.EnsureIndex(Util.GetElementNameFor<UserIdentity>(_ => _.UserName));
        }

        public Task CreateAsync(TUser user)
        {
            return Task.Run(() => UsersCollection.Insert(user));
        }

        public Task UpdateAsync(TUser user)
        {
            return Task.Run(() => UsersCollection.Save(user));
        }

        public Task DeleteAsync(TUser user)
        {
            var query = new QueryDocument();
            query.AddRange(new Dictionary<string, object>
            {
                {"_id", user.Id}
            });
            return Task.Run(() => UsersCollection.Remove(query));
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            return Task.Run(() => UsersCollection.FindOneByIdAs<TUser>(userId));
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return Task.Run(() => UsersCollection.AsQueryable<TUser>().SingleOrDefault(x => x.UserName == userName));
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
                var identity = UsersCollection.FindOneById(user.Id);
                return identity == null ? null : identity.PasswordHash;
            });
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.Run(() =>
            {
                var identity = UsersCollection.FindOneById(user.Id);
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
                var identity = UsersCollection.FindOneById(user.Id);
                return identity == null ? new List<string>() : identity.Roles as IList<string>;
            });
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            return Task.Run(() =>
            {
                var identity = UsersCollection.FindOneById(user.Id);
                return identity != null && identity.Roles.Contains(role);
            });
        }

        public void Dispose()
        {
            _database.Server.Disconnect();
        }
    }
}