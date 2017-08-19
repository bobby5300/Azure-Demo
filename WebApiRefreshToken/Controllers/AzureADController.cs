using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace WebApiRefreshToken.Controllers
{
    [Authorize]
    public class AzureADController : ApiController
    {
        public string Get()
        {
            
            return "Heelo";
        }
    

    }
}
