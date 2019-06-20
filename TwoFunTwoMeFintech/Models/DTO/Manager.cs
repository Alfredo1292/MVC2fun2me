using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ModeAuthentication.Models.Utilitarios
{
    public class Manager
    {
        public static HttpResponseMessage ClientPostRequest(Object ObjInvoke, string uri, string baseUri)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(uri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(baseUri, ObjInvoke).Result;
                    return response;
                }
            }
            catch (Exception ex)
            {
                var parameters = (from property in ObjInvoke.GetType().GetProperties()
                                  select string.Concat(property.Name, "=", property.GetValue(ObjInvoke)));
                var trace = string.Format("OBJETO: {0} URI: {1} BASEURI: {2}", string.Join(",", parameters), uri, baseUri);


                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("ERROR"),
                    ReasonPhrase = ex.Message
                };
                return resp;
            }
        }

        public static async Task<HttpResponseMessage> SendRequestAsync( string uri, string baseUri, Object ObjInvoke)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                StringContent httpConent = new StringContent(JsonConvert.SerializeObject(ObjInvoke));

                HttpResponseMessage responseMessage = null;
                try
                {
                    httpClient.BaseAddress = new Uri(uri);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    responseMessage = await httpClient.PostAsync(baseUri, httpConent);
                }
                catch (Exception ex)
                {
                    if (responseMessage == null)
                    {
                        responseMessage = new HttpResponseMessage();
                    }
                    responseMessage.StatusCode = HttpStatusCode.InternalServerError;
                    responseMessage.ReasonPhrase = string.Format("RestHttpClient.SendRequest failed: {0}", ex);
                }
                return responseMessage;
            }
        }
    }
}
