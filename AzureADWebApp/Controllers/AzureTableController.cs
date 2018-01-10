using Azure;
using AzureADWebApp.CustomClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzureADWebApp.Controllers
{
    public class AzureTableController : Controller
    {
        // GET: AzureTable
        public ActionResult Index()
        {
            // Read From Table
            var a = new AzureTableStorage<LeadEntity>("LeadDataInfo");
            var ab = a.ReadAll("LeadData");

            foreach (var item in ab)
            {
                var LeadData = item.LeadData;
                LeadInfo objleadinfo = new LeadInfo();
                objleadinfo = JsonConvert.DeserializeObject<LeadInfo>(LeadData as string);

                //GetValues(objleadinfo);

                var a1 = Task.Run<string>(() => PostLeadData(objleadinfo)).Result;

                //using (var client = new HttpClient())
                //{

                //    client.BaseAddress = new Uri("http://localhost:1565/");
                //    var response = client.PostAsJsonAsync("api/person", objleadinfo).Result;
                //    if (response.IsSuccessStatusCode)
                //    {
                //        Console.Write("Success");
                //    }
                //    else
                //        Console.Write("Error");
                //}

                ViewBag.message = a1;

            }

            return View();
        }
        
        static async Task<string> PostLeadData(LeadInfo objLead)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59496/");                
                //var Leadcontent = new FormUrlEncodedContent(GetValues(objLead));
                var result = await client.PostAsJsonAsync("/api/Home", objLead);
                var a = result.StatusCode;
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
                
            }
        }

        public static IDictionary<string, string> GetValues(object obj)
        {
            return obj
                    .GetType()
                    .GetProperties()
                    .ToDictionary(p => p.Name, p => p.GetValue(obj).ToString());
        }


        public class LeadInfo
        {
            public string name { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string area { get; set; }
            public string society { get; set; }
            public string address { get; set; }
            public string task { get; set; }
            public string leadlampid { get; set; }
            public string USERNAME { get; set; }
            public string PASSWORD { get; set; }


        }
    }
}