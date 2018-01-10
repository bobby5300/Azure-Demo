using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace RetryPattern
{
    class Program
    {
        public static void Main(string[] args)
        {
            //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                HttpClient objClient = new HttpClient();

                Task.Run(async () =>
                {
                    // Do any async anything you need here without worry
                    var maxRetryAttempts = 4;
                    var pauseBetweenFailures = TimeSpan.FromSeconds(30);
                    var requestData = new StringContent("Hello", Encoding.UTF8, "application/json");
                    await RetryHelper.RetryOnExceptionAsync<HttpRequestException>
                   (maxRetryAttempts, pauseBetweenFailures, async () =>
                        {
                            var response = await objClient.PostAsync(
                                "http://180.151.100.73:8077/api/CRMResp1", requestData);
                            response.EnsureSuccessStatusCode();
                            var a = response.Content.ReadAsStringAsync().Result;

                        });
                }).GetAwaiter().GetResult();

                //Retry.Do(GetID, TimeSpan.FromSeconds(1), 4);

                //using (var client = new HttpClient(new RetryHandler(new HttpClientHandler())))
                //{
                //    var myResult = client.GetAsync("https://snetdev.spectranet.in:8088/api/Values").Result;
                //}


            }
            catch (Exception ex)
            {

                throw ex;
            }

            
        }

        public class RetryHandler : DelegatingHandler
        {
            // Strongly consider limiting the number of retries - "retry forever" is
            // probably not the most user friendly way you could respond to "the
            // network cable got pulled out."
            private const int MaxRetries = 3;

            public RetryHandler(HttpMessageHandler innerHandler)
                : base(innerHandler)
            { }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                HttpResponseMessage response=  new HttpResponseMessage();
                for (int i = 0; i < MaxRetries; i++)
                {
                    response = await base.SendAsync(request, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }

                return response;
            }
        }

        static async Task<string> DownloadPage(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var r = await client.GetAsync(new Uri(url)))
                    {
                        string result = await r.Content.ReadAsStringAsync();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }        

        public static async void GetID()
        {            
            var r = await DownloadPage("https://snetdev.spectranet.in:8088/api/Values");
            Console.WriteLine(r.ToString());
            //var a = response.ReadAsStringAsync().Result;
        }
    }
}
