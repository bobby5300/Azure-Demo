using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolyRetyPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                // Asynchronous demos
                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                //PollyTestClient.Samples.AsyncDemo01_RetryNTimes.ExecuteAsync(cancellationToken).Wait();
                PollyTestClient.Samples.AsyncDemo02_WaitAndRetryNTimes.ExecuteAsync(cancellationToken).Wait();

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public static void Execute()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().DeclaringType.Name);
            Console.WriteLine("=======");

            // Let's call a web api service to make repeated requests to a server. 
            // The service is programmed to fail after 3 requests in 5 seconds.

            var client = new WebClient();
            int eventualSuccesses = 0;
            int retries = 0;
            int eventualFailures = 0;
            // Define our policy:

            //var policy = Policy.Handle<Exception>().WaitAndRetryForever(
            //    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200), // Wait 200ms between each try.
            //    onRetry: (exception, calculatedWaitDuration) =>
            var policy = Policy.Handle<Exception>().Retry(3, (exception, attempt) =>
            {
                // This is your new exception handler! 
                // Tell the user what they've won!
                Console.WriteLine("Policy logging: " + exception.Message, ConsoleColor.Yellow);
                retries++;

            });

            int i = 0;
            // Do the following until a key is pressed
            while (!Console.KeyAvailable)
            {
                i++;

                try
                {
                    // Retry the following call according to the policy - 3 times.
                    policy.Execute(() =>
                    {
                        // This code is executed within the Policy 

                        // Make a request and get a response
                        var msg = client.DownloadString("https://google.com/" + i);

                        // Display the response message on the console
                        Console.WriteLine("Response : " + msg, ConsoleColor.Green);
                        eventualSuccesses++;

                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine("Request " + i + " eventually failed with: " + e.Message, ConsoleColor.Red);
                    eventualFailures++;
                }

                // Wait half second
                Thread.Sleep(500);
            }

            Console.WriteLine("");
            Console.WriteLine("Total requests made                 : " + i);
            Console.WriteLine("Requests which eventually succeeded : " + eventualSuccesses);
            Console.WriteLine("Retries made to help achieve success: " + retries);
            Console.WriteLine("Requests which eventually failed    : " + eventualFailures);
            

        }
    }
}
