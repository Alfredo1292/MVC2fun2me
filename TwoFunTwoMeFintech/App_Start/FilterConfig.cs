using System.Web.Mvc;

namespace TwoFunTwoMeFintech
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
<<<<<<< HEAD
            //filters.Add(new AuthorizeAttribute());
        }                  
=======
        }
                  
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
    }
}