using System.Collections.Generic;
using System.Threading.Tasks;
using SnakeBattleNet.Web.Core.Auth;

namespace SnakeBattleNet.Web.Core
{
    public interface IUserSearch<TUser> where TUser : UserIdentity
    {
        Task<IEnumerable<TUser>> FindUsersAsync(string userNamePart, int skip, int take);
    }
}