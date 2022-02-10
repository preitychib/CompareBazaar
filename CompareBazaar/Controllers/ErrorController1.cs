using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareBazaar.Controllers
{
    public class ErrorController1 : Controller
    {
        [Route("Error/{stausCode}")]
        public IActionResult Error(int statusCode)
        {
            switch (statusCode)
            { 
                case 404:
                    ViewData["ErrorMessage"] = "WE COULDN'T FIND THAT PAGE.";
                    break;

            }

            return View("~/Views/Home/Error");
        }
    }
}
