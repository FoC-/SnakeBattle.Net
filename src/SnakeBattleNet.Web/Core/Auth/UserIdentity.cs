using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace SnakeBattleNet.Web.Core.Auth
{
    public class UserIdentity : IUser<string>
    {
        public string Id { get; private set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<string> Roles { get; set; }

        public UserIdentity()
        {
            Roles = new List<string>();
            Id = Guid.NewGuid().ToString().ToLower().Replace("-", "");
        }

        public UserIdentity(string userName)
            : this()
        {
            UserName = userName;
        }
    }
}