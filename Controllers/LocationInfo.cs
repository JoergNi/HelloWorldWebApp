using System.Net;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Web;
using HelloWorldWebApp.Controllers;

public class LocationInfo
{

    private XmlDocument _ipInfoXML;
    public LocationInfo(string ipAddress)
    {
        string url = "http://ip-api.com/xml/";
        string checkURL = url + ipAddress;
        string ipResponse = RequestHelper(checkURL);

        _ipInfoXML = new XmlDocument();
        _ipInfoXML.LoadXml(ipResponse);
       
    }


    public string GetCountryName()
    {
        XmlNodeList responseXML = _ipInfoXML.GetElementsByTagName("country");
        return responseXML.Item(0).InnerText;
    }


    public string GetCountryCode()
    {
        XmlNodeList responseXML = _ipInfoXML.GetElementsByTagName("countryCode");
        return responseXML.Item(0).InnerText;
    }

    public static string GetLocationInfo(string address)
    {
        string encodedAddress = HttpUtility.UrlEncode(address);
        string url = " http://dev.virtualearth.net/REST/v1/Locations/"+encodedAddress+ "?o=xml&incl=ciso2&key=Ag2p3wHuXnmlaO-LffokUlisExjYT6n70Vo9t71n72V5qQ_ZqA6gmPT4cXuD1Ych";
        string result = RequestHelper(url);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(result);
        XmlNode resourceSet = xmlDocument.GetElementsByTagName("ResourceSet")[0];
        int numberOfResults = int.Parse(xmlDocument.GetElementsByTagName("EstimatedTotal").Item(0).InnerText);

        string country = xmlDocument.GetElementsByTagName("CountryRegionIso2").Item(0).InnerText;
        string languageCode = TranslatorAccess.GetLanguageCode(country);
        string localizedResult = RequestHelper(url+"&c="+ languageCode);
        XmlDocument localizedXmlDocument = new XmlDocument();
        localizedXmlDocument.LoadXml(localizedResult);
        string localizedAddress = localizedXmlDocument.GetElementsByTagName("Address").Item(0).InnerText;
        return localizedAddress;
    }

    public static string RequestHelper(string url)
    {
      

        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

        StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
        string responseRead = responseStream.ReadToEnd();

        responseStream.Close();
        responseStream.Dispose();

        return responseRead;
    }
}
