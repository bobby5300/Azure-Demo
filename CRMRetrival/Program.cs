using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using CRM;
using Microsoft.Xrm.Sdk.Query;

namespace CRMRetrival
{
    class Program
    {
        static void Main(string[] args)
        {
            GetCaseDetails("crm.deploy", "Crdm@311#", "9000218229");
        }

        public static void GetCaseDetails(string USERNAME, string PASSWORD, string phone)
        {

            bool authenticated = true;//CRM.CRM_Connection.Authenticate(USERNAME, PASSWORD);
            Responce3 Resp = new Responce3();
            EntityCollection enc = null;
            if (authenticated)
            {

                IOrganizationService service = CRM_Connection.Get_Service();// check the connections in web.config
                if (service != null)
                {
                    try
                    {   

                        QueryExpression query = new QueryExpression("account");
                        query.ColumnSet.AddColumns("alletech_accountid", "name", "alletech_mobilephone");
                        FilterExpression filter = new FilterExpression(LogicalOperator.Or);
                        filter.Conditions.Add(new ConditionExpression("telephone1", ConditionOperator.Equal, phone));
                        filter.Conditions.Add(new ConditionExpression("alletech_mobilephone", ConditionOperator.Equal, phone));
                        query.Criteria = filter;

                        // Pass query to service proxy 
                        EntityCollection account = service.RetrieveMultiple(query);                        
                        if (account.Entities.Count > 0)
                        {
                            var a = account;
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public class Responce3
        {
            //public List<string> _list { get; set; }

            public string srNumber { get; set; }
            public string createdon { get; set; }
            public string description { get; set; }
            public string status { get; set; }
            public string modifiedon { get; set; }
            public string resolutionStatus { get; set; }
            public string errorMsg { get; set; }
        }
    }
}
