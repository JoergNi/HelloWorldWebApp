using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloWorldWebApp.Controllers
{
    public class HomeController : Controller
    {
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
            ViewBag.Message = "Your application description page.";

            return View();
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
            
            ViewBag.Message = LocationInfo.GetLocationInfo(addressInput.Address);
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