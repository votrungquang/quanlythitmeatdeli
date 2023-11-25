using System.Web.Mvc;

namespace WebBanHangMeatDeli
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
