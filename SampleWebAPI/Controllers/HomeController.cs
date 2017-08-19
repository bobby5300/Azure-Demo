using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SampleWebAPI.Controllers
{
    public class HomeController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public async Task<string> Post(JObject objData)
        {
            int eventualSuccesses = 0;
            int retries = 0;
            int eventualFailures = 0;
            string result = string.Empty;

            try
            {
                HttpClient client = new HttpClient();

                dynamic d = JsonConvert.DeserializeObject(objData.ToString());

                string s = d.SourceData;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var Info = d.LeadData.ToString();

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("LeadData", Info);
                
                string json = JsonConvert.SerializeObject(dictionary);
                var requestData = new StringContent(json, Encoding.UTF8, "application/json");

                
                // Define our policy:
                var policy = Policy.Handle<Exception>().WaitAndRetryAsync(
                    retryCount: 3, // Retry 3 times
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200), // Wait 200ms between each try.
                    onRetry: (exception, calculatedWaitDuration) => // Capture some info for logging!
                    {
                    // This is your new exception handler! 
                    // Tell the user what they've won!
                    System.Diagnostics.Debug.WriteLine("Policy logging: " + exception.Message, ConsoleColor.Yellow);
                        retries++;

                    });

                int i = 0;

                await policy.ExecuteAsync(async () =>
                {
                // This code is executed within the Policy 

                // Make a request and get a response
                var response = await client.PostAsync(
                              "http://180.151.100.73:8077/api/CRMResp1", requestData);
                    response.EnsureSuccessStatusCode();
                    result = response.Content.ReadAsStringAsync().Result;

                // Display the response message on the console
                //Console.WriteLine("Response : " + msg, ConsoleColor.Green);
                eventualSuccesses++;
                });
            }
            catch (Exception ex)
            {

                eventualFailures++;
            }

            // Wait half second
            await Task.Delay(TimeSpan.FromSeconds(0.5));

            return result;

        }
    }
}
