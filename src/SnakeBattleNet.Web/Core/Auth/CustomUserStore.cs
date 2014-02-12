using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
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

        public void Dispose()
        {
        }
    }
}