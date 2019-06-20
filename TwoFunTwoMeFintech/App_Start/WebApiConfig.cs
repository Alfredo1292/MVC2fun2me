using System.Web.Http;

namespace TwoFunTwoMeFintech
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            // config.Routes.MapHttpRoute(
            //     name: "DefaultApi",
            //     routeTemplate: "api/{controller}/{id}",
            //     defaults: new { id = RouteParameter.Optional }
            // );

            // config.Routes.MapHttpRoute(
            //    name: "Default",
            //    routeTemplate: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Buckets", action = "Bucket", id = UrlParameter.Optional }
            //);
        }
    }
}
