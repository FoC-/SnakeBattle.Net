using System.Web.Mvc;
using SnakeBattleNet.FE.Filters;

namespace SnakeBattleNet.FE.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogonAuthorize());
            filters.Add(new HandleErrorAttribute());
        }
    }
}