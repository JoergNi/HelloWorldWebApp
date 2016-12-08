using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloWorldWebApp.Controllers
{
    public static class DropdownHelper
    {
        public static IEnumerable<SelectListItem> GetAllCountries()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Germany", Value = "de" });

            items.Add(new SelectListItem { Text = "India", Value = "in" });

            items.Add(new SelectListItem { Text = "HongKong", Value = "hk", Selected = true });



            return items;
        }
    }
}