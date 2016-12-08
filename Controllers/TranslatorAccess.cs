using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelloWorldWebApp.Controllers
{


    public static class TranslatorAccess
    {
        public static IDictionary<DataInput, string> Translations = new Dictionary<DataInput, string>();
        public static IDictionary<string, string> CountryCodeToLanguageCode = new Dictionary<string, string>();

        private const string SubscriptionKey = "2033bf855ec24c79b47102716f4aa11c";

        private static AzureAuthToken _azureAuthToken;

        public static AzureAuthToken AzureAuthToken
        {
            get
            {
                if (_azureAuthToken == null)
                {
                    _azureAuthToken = new AzureAuthToken(SubscriptionKey);
                }
                return _azureAuthToken;
            }
        }


        private static IList<string> _languagesForTranslate;

        public static IList<string> LanguagesForTranslate
        {
            get
            {
                if (_languagesForTranslate == null)
                {
                    _languagesForTranslate = GetLanguagesForTranslate();
                }
                return _languagesForTranslate;
            }
        }

        public static string TranslateByCountryCode(string textToTranslate, string countryCode)
        {
            var dataInput = new DataInput()
            {
                Text = textToTranslate,
                SelectedCountry = countryCode
            };
            string result;
            if (!Translations.TryGetValue(dataInput, out result))
            {


                string languageCode = GetLanguageCode(countryCode);
                result = Translate(textToTranslate, languageCode);
                if (Translations.Count > 100000)
                {
                    Translations.Clear();
                }
                Translations[dataInput] = result;

            }
            return result;
        }

        public static string Translate(string textToTranslate, string toLanguageCode)
        {
            string result;
            if (!LanguagesForTranslate.Any(x => x.Equals(toLanguageCode, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception("Language " + toLanguageCode + " not supported.");
            }

            string txtToTranslate = textToTranslate;
            string uri = string.Format("http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(txtToTranslate) + "&to={0}", toLanguageCode);
            WebRequest translationWebRequest = WebRequest.Create(uri);
            translationWebRequest.Headers.Add("Authorization", AzureAuthToken.GetAccessToken());

            using (WebResponse response = translationWebRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    Encoding encode = Encoding.GetEncoding("utf-8");
                    using (StreamReader translatedStream = new StreamReader(stream, encode))
                    {
                        System.Xml.XmlDocument xTranslation = new System.Xml.XmlDocument();
                        xTranslation.LoadXml(translatedStream.ReadToEnd());
                        result = xTranslation.InnerText;
                    }
                }
            }

            return result;
        }

        public static string GetLanguageCode(string countryCode)
        {
            string result;
            if (!CountryCodeToLanguageCode.TryGetValue(countryCode, out result))
            {
                //TODO better exception if country not found           
                string uri = string.Format("https://restcountries.eu/rest/v1/alpha/" + countryCode);

                WebRequest countryInfoWebRequest = WebRequest.Create(uri);

                CountryInfo countryInfo;
                using (WebResponse response = countryInfoWebRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {

                        DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(CountryInfo));
                        countryInfo = (CountryInfo)dcs.ReadObject(stream);

                    }
                }
                result = countryInfo.languages.First();
                if (countryCode.Equals("cn", StringComparison.InvariantCultureIgnoreCase)) result = "zh-CHS";

                if (countryCode.Equals("hk", StringComparison.InvariantCultureIgnoreCase)) result = "zh-CHT";
                CountryCodeToLanguageCode[countryCode] = result;
            }
            return result;
        }



        private static IList<string> GetLanguagesForTranslate()
        {
            IList<string> languagesForTranslate;
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForTranslate";
            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.Headers.Add("Authorization", AzureAuthToken.GetAccessToken());


            using (WebResponse response = webRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {

                    DataContractSerializer dcs = new DataContractSerializer(typeof(List<string>));
                    languagesForTranslate = (List<string>)dcs.ReadObject(stream);
                    var friendlyName = languagesForTranslate.ToArray(); //put the list of language codes into an array to pass to the method to get the friendly name.

                }
            }
            return languagesForTranslate;
        }


    }


}