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
using TranslationRobot;

namespace HelloWorldWebAppTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddTranslation()
        {
            var translationInput = new TranslationInput() {Input = "Test", Translation = "广东省广州市" };
            var postTranslation = HomeController.PostTranslation(translationInput, "http://localhost:24615/api/AddTranslation");
            Assert.AreEqual("asd",postTranslation);
        }
       
    }
}
