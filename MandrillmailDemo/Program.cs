using Log4net;
using Mandrill;
using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MandrillmailDemo
{
    class Program
    {
        
        static void Main(string[] args)
        {

            ILogService logService = new FileLogService(typeof(Program));

            logService.Info("Hello123");

            //var api = new MandrillApi("KRMjtbzqhDjIIqWFN_sNug");

            //var EmailID = ConfigurationManager.AppSettings["Emails"];

            //var Temp = EmailID.Split(',');

            //for (int i = 0; i < Temp.Length; i++)
            //{
            //    var message = new MandrillMessage("care@spectranet.in", Temp[i].ToString(),"hello mandrill!", "...how are you?");

            //    var result = api.Messages.SendAsync(message).Result;
            //}

            

        }
    }
}
