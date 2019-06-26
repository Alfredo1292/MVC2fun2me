using TwoFunTwoMe.Controllers;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace TwoFunTwoMe
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Filters.Add(new ValidateModelAttribute());
			config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));
			//GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("type", "octet-stream", new MediaTypeHeaderValue("application/octet-stream")));
			GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
			//aplica seguridad a wsTuca crea SSL/TLS secure channel.
			bool flag = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["ValidaProtocolosSSLTSL"], out flag);
            if (flag)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
    }
}
