using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CRM
{
    public class CRM_Connection
    {
        public static IOrganizationService Get_Service()
        {
            try
            {
                ClientCredentials Credentials = new ClientCredentials();
                Credentials.UserName.UserName = ConfigurationManager.AppSettings["Domain"]+@"\"+ConfigurationManager.AppSettings["User"];
                Credentials.UserName.Password = ConfigurationManager.AppSettings["Password"];

                Uri OrganizationUri = new Uri(ConfigurationManager.AppSettings["orgurl"]);
                Uri HomeRealmUri = null;

                using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, HomeRealmUri, Credentials, null))
                {
                    serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());
                    IOrganizationService service = (IOrganizationService)serviceProxy;
                    OrganizationServiceContext orgContext = new OrganizationServiceContext(service);
                    return service;
                }
            }
            catch (Exception ec)
            {
                return null;
            }
        }

        //public static IOrganizationService Get_Service2()
        //{
        //    try
        //    {
        //        ClientCredentials Credentials = new ClientCredentials();
        //        Credentials.UserName.UserName = ConfigurationManager.AppSettings["Domain"] + @"\" + ConfigurationManager.AppSettings["User"];
        //        Credentials.UserName.Password = ConfigurationManager.AppSettings["Password"];
                

        //        Uri OrganizationUri = new Uri(ConfigurationManager.AppSettings["orgurl2"]);
        //        Uri HomeRealmUri = null;

        //        using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, HomeRealmUri, Credentials, null))
        //        {
        //            serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());
        //            IOrganizationService service = (IOrganizationService)serviceProxy;
        //            OrganizationServiceContext orgContext = new OrganizationServiceContext(service);
        //            return service;
        //        }
        //    }
        //    catch (Exception ec)
        //    {
        //        return null;
        //    }
        //}

        public static EntityReference GetEntityRecord(IOrganizationService service, string Entity_Name, string Entity_Attribute, string Field_Value)
        {
            try
            {
                QueryExpression query = new QueryExpression(Entity_Name.ToLower());
                query.ColumnSet = new ColumnSet(true);
                ConditionExpression condi = new ConditionExpression(Entity_Attribute.ToLower(), ConditionOperator.Equal, Field_Value);
                query.Criteria.AddCondition(condi);


                EntityCollection Collection = service.RetrieveMultiple(query);

                if (Collection.Entities.Count > 0)
                {
                    Entity obj_entity = (Entity)Collection.Entities[0];
                    EntityReference reference = new EntityReference(Entity_Name.ToLower(), obj_entity.Id);

                    return reference;
                }

                return null;
            }
            catch (Exception ec)
            {
                throw new Exception(ec.ToString());

            }
        }
        public static EntityReference GetEntityRecordwith2filters(IOrganizationService service, string Entity_Name, string Entity_Attribute1, string Entity_Attribute2, string Field_Value1, string Field_Value2)
        {
            try
            {
                QueryExpression query = new QueryExpression(Entity_Name.ToLower());
                query.ColumnSet = new ColumnSet(true);
                ConditionExpression condi = new ConditionExpression(Entity_Attribute1.ToLower(), ConditionOperator.Equal, Field_Value1);
                ConditionExpression condi2 = new ConditionExpression(Entity_Attribute2.ToLower(), ConditionOperator.Equal, Field_Value2);
                query.Criteria.AddCondition(condi);
                query.Criteria.AddCondition(condi2);


                EntityCollection Collection = service.RetrieveMultiple(query);

                if (Collection.Entities.Count > 0)
                {
                    Entity obj_entity = (Entity)Collection.Entities[0];
                    EntityReference reference = new EntityReference(Entity_Name.ToLower(), obj_entity.Id);

                    return reference;
                }

                return null;
            }
            catch (Exception ec)
            {
                throw new Exception(ec.ToString());

            }
        }
        public static EntityCollection GetRelatedEntities(IOrganizationService service, string Entity_name, string filter_attribute, Guid channelid)
        {
            QueryExpression query = new QueryExpression(Entity_name.ToLower());
            query.ColumnSet = new ColumnSet(true);
            ConditionExpression condi = new ConditionExpression(filter_attribute.ToLower(), ConditionOperator.Equal, channelid);
            query.Criteria.AddCondition(condi);
            EntityCollection Collection = service.RetrieveMultiple(query);
            if (Collection.Entities.Count > 0)
            {
                return Collection;
            }
            else
                return null;

        }

        public static bool IsEmail(string email)
        {
            string MatchEmailPattern =
                                      @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }
        public static SqlConnection Getsqlconnection()
        {
            try
            {
                string connsctionstring = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
                SqlConnection con = new SqlConnection(connsctionstring);
                return con;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Authenticate(string username, string password)
        {
            SqlConnection sqlcon = Getsqlconnection();
            try
            {

                sqlcon.Open();
                if (sqlcon != null)
                {
                    string AuthCommAPISQLquery = "select * from Auth_Comm_Lead where UserName ='" + username + "' and password = '" + password + "'";
                    SqlDataAdapter da = new SqlDataAdapter(AuthCommAPISQLquery, sqlcon);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            try
                            {
                                return true;
                            }
                            catch (Exception ex)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                    sqlcon.Close();
                    return false;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        //code written by vallab.
        /// <summary>
        /// Get Option set value text from CRM
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="fieldName"></param>
        /// <param name="optionSetValue"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static string GetOptionSetValueLabel(string entityName, string fieldName, int optionSetValue, IOrganizationService service)
        {

            var attReq = new RetrieveAttributeRequest();
            attReq.EntityLogicalName = entityName;
            attReq.LogicalName = fieldName;
            attReq.RetrieveAsIfPublished = true;

            var attResponse = (RetrieveAttributeResponse)service.Execute(attReq);
            var attMetadata = (EnumAttributeMetadata)attResponse.AttributeMetadata;

            return attMetadata.OptionSet.Options.Where(x => x.Value == optionSetValue).FirstOrDefault().Label.UserLocalizedLabel.Label;

        }

    }
}