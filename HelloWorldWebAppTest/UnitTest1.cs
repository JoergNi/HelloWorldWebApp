using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelloWorldWebApp.Controllers;
using System.Windows;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.IO;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HelloWorldWebAppTest
{
    [TestClass]
    public class UnitTest1
    {
        private const string SubscriptionKey = "2033bf855ec24c79b47102716f4aa11c";
        [TestMethod]
        public void TestTranslate()
        {
            var translatorAccess = new TranslatorAccess();
            string result = translatorAccess.TranslateByCountryCode("Guten Morgen", "us");
            Assert.AreEqual("Good morning", result);
             result = translatorAccess.TranslateByCountryCode("Guten Morgen", "gb");
            Assert.AreEqual("Good morning", result);
             result = translatorAccess.TranslateByCountryCode("Guten Morgen", "au");
            Assert.AreEqual("Good morning", result);
            Assert.AreEqual("Good morning", result);
            result = translatorAccess.TranslateByCountryCode("Guten Morgen", "de");
            Assert.AreEqual("Guten Morgen", result);
            result = translatorAccess.TranslateByCountryCode("Guten Morgen", "co");
            Assert.AreEqual("Buenos días", result);
            result = translatorAccess.TranslateByCountryCode("Guten Morgen", "hk");
            Assert.AreEqual("早上好", result);
           



        }

        [TestMethod]
        public void TestGetLanguageCode()
        {
            var translatorAccess = new TranslatorAccess();
            string result = translatorAccess.GetLanguageCode("de");
            Assert.AreEqual("de", result);
            result = translatorAccess.GetLanguageCode("us");
            Assert.AreEqual("en", result);
            result = translatorAccess.GetLanguageCode("at");
            Assert.AreEqual("de", result);

        }


        [TestMethod]
        public void TestGetLocationInfo()
        {
            var translatorAccess = new TranslatorAccess();
            string result = LocationInfo.GetLocationInfo("Cologne", translatorAccess);
            Assert.AreEqual("Köln, NW, Deutschland", result);

            result = LocationInfo.GetLocationInfo("Bonner Str 17, Cologne", translatorAccess);
            Assert.AreEqual("Bonner Straße 17, 50677 Köln, Deutschland", result);
            
            result = LocationInfo.GetLocationInfo("Bonner, Str, 17, Cologne", translatorAccess);
            Assert.AreEqual("Bonner Straße 17, 50677 Köln, Deutschland", result);

            result = LocationInfo.GetLocationInfo("1 Queen's Road Central, Hongkong", translatorAccess);
            Assert.AreEqual("Bonner Straße 17, 50677 Köln, Deutschland", result);
           
        }
    }
}
