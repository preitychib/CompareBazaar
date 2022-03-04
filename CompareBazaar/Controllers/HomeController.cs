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
      //  private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
           // _client = client;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult FAQs()
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

        public IActionResult SearchResults()
        {
            return View();
        }
        public async Task<IActionResult> ProductsListAsync()
        {
            ViewBag.mobiles = await GetMobiles("flipkart");
            return View();
           
        }
        private static async Task<object> GetMobiles(string vendor)
        {
            try
            {
                using var client = new HttpClient();
                var Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile/?ordering=-price";

                // return await client.GetAsync(url);
                var Response = await client.GetAsync(Url);
                Response.EnsureSuccessStatusCode();

                var Content = await Response.Content.ReadAsStringAsync();

                var Mobiles = JsonConvert.DeserializeObject<object>(Content);


                // var newMobiles = await _mobileService.GetMobiles();
                return Mobiles;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return null;
            }
        }
        public async Task<IActionResult> ProductDetailsAsync(string vendor,int id)
        {
            //int id = (int)TempData["id"];

            using var client = new HttpClient();
            var url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile/{id}";

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
