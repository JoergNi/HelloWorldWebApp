using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using TranslationRobot;

namespace HelloWorldWebApp.Controllers
{
    public class HomeController : Controller
    {

        private TranslatorAccess TranslatorAccess
        {
            get
            {
                TranslatorAccess result;
                HttpContext context = System.Web.HttpContext.Current;
                if (context.Application.Contents["TranslatorAccess"] != null)
                {
                    result = context.Application.Contents["TranslatorAccess"] as TranslatorAccess;
                }
                else
                {
                    result = new TranslatorAccess();
                    context.Application.Contents["TranslatorAccess"] = result; 
                }
                return result;
            }
        }

        public ActionResult Index()
        {
            var locationInfo = GetLocationInfo();
           
            ViewBag.Message = "Hello to " + locationInfo.GetCountryName() + "\r\n";
            try
            {
                string countryCode = locationInfo.GetCountryCode();
                string languageCode = TranslatorAccess.GetLanguageCode(countryCode);
                string personalized = TranslatorAccess.Translate("Welcome to our Website", languageCode);
                ViewBag.SubMessage += personalized;
            }
            catch (Exception e)
            {
                ViewBag.SubMessage += e.Message;
            }



            ;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.\r\n";
            CloudTable table = GetTable();
            var query = table.CreateQuery<TranslatedAddressEntity>();
            var queryResult = table.ExecuteQuery(query);
            foreach (var item in queryResult)
            {
                ViewBag.Message += item.RowKey + " = " + item.Translation+"\r\n";
            }

            return View();
        }


        public CloudTable GetTable()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference(TranslatedAddressEntity.TableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            return table;

        }

        public ActionResult Result(DataInput dataInput)
        {
            string countryCode = dataInput.SelectedCountry;
            if (string.IsNullOrWhiteSpace(dataInput.SelectedCountry))
            {
                var locationInfo = GetLocationInfo();
                countryCode = locationInfo.GetCountryCode();
            }

            string languageCode = TranslatorAccess.GetLanguageCode(countryCode);

            ViewBag.Message = TranslatorAccess.Translate(dataInput.Text, languageCode);
            return View();
        }

        public ActionResult LocalizeAddress(AddressInput addressInput)
        {
            string encodedAddress = System.Uri.EscapeUriString(addressInput.Address);
            string uri = string.Format("http://webrole1.azurewebsites.net/api/Translate/" + encodedAddress);
            string result = RequestHelper.DownloadString(uri);

            ViewBag.Message = result;
          
            return View();
        }

        public ActionResult Test()
        {

            string message = "Robots are smart. ";
            ViewBag.Message = TranslatorAccess.TranslateByCountryCode(message, "de");
            ViewBag.Message += TranslatorAccess.TranslateByCountryCode(message, "hk");
            ViewBag.Message += TranslatorAccess.TranslateByCountryCode(message, "us");
            ViewBag.Message += TranslatorAccess.TranslateByCountryCode(message, "in");
            ViewBag.Message += TranslatorAccess.TranslateByCountryCode(message, "fr");
            ViewBag.Message += TranslatorAccess.TranslateByCountryCode(message, "cn");
            ViewBag.Message += TranslatorAccess.TranslateByCountryCode(message, "br");
            ViewBag.Message += TranslatorAccess.TranslateByCountryCode(message, "th");




            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Created by me.";


            return View();
        }

        private LocationInfo GetLocationInfo()
        {

            var ip = GetIPAddress();
            if (string.IsNullOrEmpty(ip))
            {
                ip = "87.176.166.69";
            }
            var locationInfo = new LocationInfo(ip);
            return locationInfo;

        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            var result = "";
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    result = addresses[0];
                }
            }
            else
            {
                result = context.Request.ServerVariables["REMOTE_ADDR"];
            }

            return result.Split(new[] { ':' }).First();
        }
    }
}