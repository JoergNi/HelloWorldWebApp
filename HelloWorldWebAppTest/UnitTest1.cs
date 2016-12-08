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
            string result = TranslatorAccess.TranslateByCountryCode("Guten Morgen", "us");
            Assert.AreEqual("Good morning", result);
             result = TranslatorAccess.TranslateByCountryCode("Guten Morgen", "gb");
            Assert.AreEqual("Good morning", result);
             result = TranslatorAccess.TranslateByCountryCode("Guten Morgen", "au");
            Assert.AreEqual("Good morning", result);
            Assert.AreEqual("Good morning", result);
            result = TranslatorAccess.TranslateByCountryCode("Guten Morgen", "de");
            Assert.AreEqual("Guten Morgen", result);
            result = TranslatorAccess.TranslateByCountryCode("Guten Morgen", "co");
            Assert.AreEqual("Buenos días", result);
            result = TranslatorAccess.TranslateByCountryCode("Guten Morgen", "hk");
            Assert.AreEqual("早上好", result);
           



        }

        [TestMethod]
        public void TestGetLanguageCode()
        {
            string result = TranslatorAccess.GetLanguageCode("de");
            Assert.AreEqual("de", result);
            result = TranslatorAccess.GetLanguageCode("us");
            Assert.AreEqual("en", result);
            result = TranslatorAccess.GetLanguageCode("at");
            Assert.AreEqual("de", result);

        }


        [TestMethod]
        public void TestGetLocationInfo()
        {
            string result = LocationInfo.GetLocationInfo("HongKong");
            Assert.AreEqual("bodo", result);

        }
    }
}
