using CompareBazaar.Models;
using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.mobiles = await GetMobiles("amazon");
           // Console.WriteLine(ViewBag.mobiles);
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

        [Authorize]
        public async Task<IActionResult> CompareChartAsync(string vendor=null, int id=-1)
        {
            
            
            if (vendor!=null && id!=-1) {
                ViewBag.mobile = await GetMobile(vendor, id);
              /*  if (TempData["compChart"] == null)
                {
                    List<CompareChart> compareItem = new List<CompareChart>();
                    compareItem.Add(new CompareChart()
                    {
                        id = id,
                        vendor = vendor
                    });
                    TempData["compChart"] = compareItem;
                }
                else
                {
                    List<CompareChart> compareItem = (List<CompareChart>)TempData["compChart"];
                    compareItem.Add(new CompareChart(){
                        id = id,
                        vendor = vendor
                    });
                    TempData["compChart"] = compareItem;
                }*/

                return View();
            }
            ViewBag.mobile = null;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> WishListAsync(string vendor = null, int id = -1)
        {
            //int id = (int)TempData["id"];

            if (vendor != null && id != -1)
            {

                List<CompareChart> compareItem = new List<CompareChart>();
                compareItem.Add(new CompareChart()
                {
                    id = id,
                    vendor = vendor
                });

                ViewBag.mobile = await GetMobile(compareItem[0].vendor, compareItem[0].id);
                return View();
            }
            ViewBag.mobile = null;

            return View();
        }
        public async Task<IActionResult> SearchResultsAsync(string searchStr)
        {
            using var client = new HttpClient();
            var Url = $"https://comparebazaar-api.herokuapp.com/api/flipkart/mobile/?search={searchStr}";

            // return await client.GetAsync(url);
            var Response = await client.GetAsync(Url);
            Response.EnsureSuccessStatusCode();

            var Content = await Response.Content.ReadAsStringAsync();

            var Mobiles = JsonConvert.DeserializeObject<object>(Content);

            ViewBag.mobiles = Mobiles;
            // var newMobiles = await _mobileService.GetMobiles();
           

            return View("./ProductsList");
        }
        public async Task<IActionResult> ProductsListAsync()
        {
            
            ViewBag.mobiles = await GetMobiles("flipkart");
            //ViewBag.mobiles = await GetMobiles("flipkart");
            
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
           
            ViewBag.mobile = await GetMobile(vendor,id);
            
            return View();
        }

        private static async Task<object> GetMobile(string vendor,int id)
        {
            try
            {
                using var client = new HttpClient();
                var Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile/{id}";

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
