using System;
using System.Collections.Generic;

namespace SnakeBattleNet.Web.Core.Auth
{
    public class RolesProvider
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static IEnumerable<String> Roles { get { return new[] { Admin, User }; } }
    }
}