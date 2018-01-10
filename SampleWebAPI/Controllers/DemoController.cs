using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SampleWebAPI.Controllers
{
    public class DemoController : ApiController
    {

        public async Task<string> Get()
        {

            return await SomethingAsync();

        }

        public async Task<string> SomethingAsync()
        {
            HttpClient client = new HttpClient();

            var res = await client.GetAsync("https://jsonplaceholder.typicode.com/posts/1");

            return await  res.Content.ReadAsStringAsync();
        }





        public string Post(JObject objData)
        {


            dynamic JsonData = JsonConvert.DeserializeObject(objData.ToString());
            var accountDetail = JsonData.accountDetail.ToString();
            var paymentDetail = JsonData.paymentDetail.ToString();
            var documentRequirement = JsonData.documentRequirement.ToString();
            var cafDetail = JsonData.cafDetail.ToString();
            var TempCAFID = JsonData.LAMPCAFID.ToString();
            var LAMPLEADID = JsonData.LAMPLEADID.ToString();

            var RespCRM = SendCRM(accountDetail, paymentDetail, documentRequirement, cafDetail, TempCAFID, LAMPLEADID);

            return RespCRM;
        }

        private string SendLead(string LAMPCAFID, string CRMRespID)
        {
            var LeadResp = Task.Run<string>(() => SendCAFRespMessage(LAMPCAFID, CRMRespID)).Result;
            //log.Info(LeadResp);
            return LeadResp;
        }

        private async Task<string> SendCAFRespMessage(string LAMPCAFID, string CRMRespID)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    dictionary.Add("LAMPCAFID", LAMPCAFID);
                    dictionary.Add("CAFInfo", CRMRespID);


                    string json = JsonConvert.SerializeObject(dictionary);
                    //log.Info(json);
                    var requestData = new StringContent(json, Encoding.UTF8, "application/json");
                    var result = "";

                    var response = await client.PostAsync(String.Format("http://180.151.100.73:8077/api/LAMP_CAF_Resp"), requestData).ConfigureAwait(false);
                    //log.Info(response.StatusCode.ToString());
                    //log.Info(response.Content.ReadAsStringAsync().Result);
                    result = response.Content.ReadAsStringAsync().ToString();
                    //log.Info(result);
                    //SendQueue(json.ToString()); 
                    if (result == "0")
                    {
                        //SendQueue(json.ToString());
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                //log.Info("SendCAFRespMessage " + ex.Message.ToString());

                return "";
            }
        }

        private string SendCRM(string accountDetail, string paymentDetail, string documentRequirement, string cafDetail, string TempCAFID, string LAMPLEADID)
        {
            var LeadResp = Task.Run<string>(() => SendCAFCRM(accountDetail, paymentDetail, documentRequirement, cafDetail, TempCAFID, LAMPLEADID)).Result;
            //log.Info("SEND CAF CRM " + LeadResp);
            return LeadResp;
        }

        private async Task<string> SendCAFCRM(string accountDetail, string paymentDetail, string documentRequirement, string cafDetail, string TempCAFID, string LAMPLEADID)
        {
            try
            {

                HttpClient client = new HttpClient();

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("accountDetail", accountDetail);
                dictionary.Add("paymentDetail", paymentDetail);
                dictionary.Add("documentRequirement", documentRequirement);
                dictionary.Add("cafDetail", cafDetail);
                dictionary.Add("LAMPLEADID", LAMPLEADID);
                dictionary.Add("LAMPCAFID", TempCAFID);
                string result = string.Empty;
                string json = JsonConvert.SerializeObject(dictionary);
                var requestData = new StringContent(json, Encoding.UTF8, "application/json");
                //log.Info("SendReq " + requestData.ToString());
                //var maxRetryAttempts = 3;
                var pauseBetweenFailures = TimeSpan.FromSeconds(30);


                var response = await client.PostAsync(
                    "http://180.151.100.73:8077/api/CAFResp", requestData);
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
                //log.Info($"Retry on CAF: {result}");


                return result;

            }
            catch (Exception ex)
            {
                //log.Info("SendLeadRespMessage1 " + ex.ToString());

                return string.Empty;
            }


        }

    }
}
