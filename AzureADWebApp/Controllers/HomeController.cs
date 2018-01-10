
using Azure;
using Microsoft.WindowsAzure.Storage.Table;
using System.Web.Mvc;
using System;

namespace AzureADWebApp.Controllers
{
    public class HomeController : Controller
    {

        // log4 net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            try
            {
                // Read From Table
                var a = new AzureTableStorage<PersonEntity>("MsgTbl");
                var ab = a.ReadAll("Queue Items");
                log.Info("Hello");

                // Get Single Record

                var ab1 = a.Find("Queue Items", "07ae0889-e688-46fb-844b-d560ea4deb65");

                //Insert Record

               

                PersonEntity person = new PersonEntity("Queue Items1", "12345");
                person.Message = "Fred Blogggs";
                var ab2 = a.Insert(person);
                
            }
            catch (Exception ex)
            {

                log.Error("This is my error", ex);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public class PersonEntity : TableEntity, IAzureStorageTable
        {
            public PersonEntity(string key, string id)
            {
                this.PartitionKey = key;
                this.RowKey = id;
                this.Timestamp = DateTime.Now;
            }

            public PersonEntity() { }
            public string Message { get; set; }

            public void SetPartitionKey(string key)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException();
                }
                this.PartitionKey = key;
            }

            public void SetRowKey(string id)
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException();
                }
                this.RowKey = id;
            }


        }
    }


}