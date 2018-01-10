using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzureADWebApp.Controllers
{
    public class AzureADController : Controller
    {

        static string DomainName = "himavlabsoutlook.onmicrosoft.com";
        static string ADInstance = "https://login.microsftonline.com/{0}";
        static string ClientID = "6331488d-5ff8-4922-80f5-c8d67a4580b6";
        static string ResourceID = "https://webpiauth.azurewebsites.net";

        static string Authority = string.Format(CultureInfo.InvariantCulture, ADInstance, DomainName);

        AuthenticationResult Result = null;
        // GET: AzureAD
        public async Task<ActionResult> Index()
        {
            if (await GetConnect())
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Result.AccessToken);

                HttpResponseMessage Response = await client.GetAsync(ResourceID + "/api/values");

                if (Response.IsSuccessStatusCode)
                {
                    string result = await Response.Content.ReadAsStringAsync();
                    ViewBag.Resp = result;
                }
            }

            return View();
        }

        public async Task<bool> GetConnect()
        {
            try
            {
                AuthenticationContext context = new AuthenticationContext("https://login.windows.net/common/oauth2/authorize");
                Result = await context.AcquireTokenAsync(ResourceID, ClientID, new Uri("http://localhost"), new PlatformParameters(PromptBehavior.Always));

                return true;
            }
            catch (Exception ex)
            {
                return false;
                
            }
        }
    }
}