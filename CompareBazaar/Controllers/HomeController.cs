using CompareBazaar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CompareBazaar.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

       

        public IActionResult CompareChart()
        {
            return View();
        }

        public IActionResult WishList()
        {
            return View();
        }

        public async Task<IActionResult> ProductsListAsync()
        {
            // return View();
            using (var client = new HttpClient())
            {
                var url = "https://comparebazaar-api.herokuapp.com/api/flipkart/mobile/";

                // return await client.GetAsync(url);
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var mobiles = JsonConvert.DeserializeObject<object>(content);

                // ViewData["response"] = response;
                Console.WriteLine(mobiles);

                ViewBag.mobiles = mobiles;

                return View();
            }
        }
        public async Task<IActionResult> ProductDetailsAsync(int id)
        {
            //int id = (int)TempData["id"];
            
            using (var client = new HttpClient())
            {
                var url = $"https://comparebazaar-api.herokuapp.com/api/flipkart/mobile/{id}";

                // return await client.GetAsync(url);
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var mobile = JsonConvert.DeserializeObject<object>(content);

                // ViewData["response"] = response;
               //Console.WriteLine(mobile);

                ViewBag.mobile = mobile;
               // ViewBag.id = id;

                return View();
            }
        }



       [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
         public IActionResult Error()
         {
             return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
         }
        

        /*[Route("Error/{stausCode}")]
        public IActionResult HttptStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "WE COULDN'T FIND THAT PAGE.";
                    break;

            }

            return View("Error");
        }*/
    }
}
