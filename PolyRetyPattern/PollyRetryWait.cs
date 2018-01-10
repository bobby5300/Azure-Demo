using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PollyTestClient.Samples
{
    /// <summary>
    /// Demonstrates the WaitAndRetry policy.
    /// Loops through a series of Http requests, keeping track of each requested
    /// item and reporting server failures when encountering exceptions.
    /// 
    /// Observations: We now have waits among the retries.
    /// In this case, still not enough wait - or not enough retries - for the underlying system to have recovered.
    /// So we still fail some calls.
    /// </summary>
    public static class AsyncDemo02_WaitAndRetryNTimes
    {
        public static async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(typeof(AsyncDemo02_WaitAndRetryNTimes).Name);
            Console.WriteLine("=======");

            // Let's call a web api service to make repeated requests to a server. 
            // The service is programmed to fail after 3 requests in 5 seconds.

            var client = new HttpClient();
            int eventualSuccesses = 0;
            int retries = 0;
            int eventualFailures = 0;
            // Define our policy:
            var policy = Policy.Handle<Exception>().WaitAndRetryAsync(
                retryCount: 3, // Retry 3 times
                sleepDurationProvider: attempt => TimeSpan.FromMinutes(1), // Wait 200ms between each try.
                onRetry: (exception, calculatedWaitDuration) => // Capture some info for logging!
                {
                    // This is your new exception handler! 
                    // Tell the user what they've won!
                    Console.WriteLine("Policy logging: " + exception.Message, ConsoleColor.Yellow);
                    retries++;

                });

            int i = 0;
            // Do the following until a key is pressed
            //while (!cancellationToken.IsCancellationRequested)
            //{
                //i++;

                try
                {
                    // Retry the following call according to the policy - 3 times.
                    await policy.ExecuteAsync(async token =>
                    {
                        // This code is executed within the Policy 

                        // Make a request and get a response
                        string msg = await client.GetStringAsync("http://localhost:45178//api/values/" + i);

                        // Display the response message on the console
                        Console.WriteLine("Response : " + msg, ConsoleColor.Green);
                        eventualSuccesses++;
                    }, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Request " + i + " eventually failed with: " + e.Message, ConsoleColor.Red);
                    eventualFailures++;
                }

                // Wait half second
                await Task.Delay(TimeSpan.FromSeconds(0.5));
            //}

            Console.WriteLine("");
            Console.WriteLine("Total requests made                 : " + i);
            Console.WriteLine("Requests which eventually succeeded : " + eventualSuccesses);
            Console.WriteLine("Retries made to help achieve success: " + retries);
            Console.WriteLine("Requests which eventually failed    : " + eventualFailures);
            Console.ReadKey();

        }
    }
}