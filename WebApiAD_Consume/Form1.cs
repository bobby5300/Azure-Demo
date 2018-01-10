using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebApiAD_Consume
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string DomainName = "himavlabsoutlook.onmicrosoft.com";
        static string ADInstance = "https://login.microsftonline.com/{0}";
        static string ClientID = "6331488d-5ff8-4922-80f5-c8d67a4580b6";
        static string ResourceID = "https://webpiauth.azurewebsites.net";

        static string Authority = string.Format(CultureInfo.InvariantCulture, ADInstance, DomainName);


        AuthenticationResult Result = null;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                AuthenticationContext context = new AuthenticationContext("https://login.windows.net/common/oauth2/authorize");
                Result = await context.AcquireTokenAsync(ResourceID, ClientID, new Uri("http://localhost"), new PlatformParameters(PromptBehavior.Always));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnGetdata_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Result.AccessToken);

            HttpResponseMessage Response = await client.GetAsync(ResourceID + "/api/values");

            if(Response.IsSuccessStatusCode)
            {
                string result = await Response.Content.ReadAsStringAsync();
                MessageBox.Show(result);
            }
        }
    }
}
