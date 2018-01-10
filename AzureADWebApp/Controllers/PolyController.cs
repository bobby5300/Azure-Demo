using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzureADWebApp.Controllers
{
    public class PolyController : Controller
    {
        readonly RetryPolicy<HttpResponseMessage> _httpRequestPolicy;
        public PolyController()
        {
            _httpRequestPolicy = Policy.HandleResult<HttpResponseMessage>(
            r => r.StatusCode == HttpStatusCode.BadRequest)
        .WaitAndRetryAsync(3,
            retryAttempt => TimeSpan.FromSeconds(retryAttempt));
        }

        public async Task<ActionResult> Get()
        {
            var httpClient = new  HttpClient();
            string requestEndpoint = "http://google.com/"; // numbers is the name of the controller being called

            HttpResponseMessage httpResponse = await _httpRequestPolicy.ExecuteAsync(() => httpClient.GetAsync(requestEndpoint));

            var numbers = await httpResponse.Content.ReadAsAsync<string>();

            return View();
        }
    }
}