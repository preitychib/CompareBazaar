using CompareBazaar.Data;
using CompareBazaar.Helpers;
using CompareBazaar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        //  private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger,UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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

        
        public async Task<IActionResult> WishListAsync(string vendor=null, int id=-1)
        {
            
            
            if (vendor!=null && id!=-1) {
               // var myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "myList");
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
      

       
        public async Task<IActionResult> CompareChartAsync()
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "myList") != null)
            {
                var myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "myList");
                int n = 0;

                // ViewBag.Mobile[n] = null;
                foreach (var product in myList)
                {
                    //ViewBag.mobile = await GetMobile(product.vendor, product.id);
                    n++;
                }

                if (n-- > 0)
                    ViewBag.mobile1 = await GetMobile(myList[0].vendor, myList[0].id);
                if (n-- > 0)
                    ViewBag.mobile2 = await GetMobile(myList[1].vendor, myList[1].id);
                if (n-- > 0)
                    ViewBag.mobile3 = await GetMobile(myList[2].vendor, myList[2].id);
                return View();
            }
           
           
                return View();
        }
       /* public async Task<IActionResult> WishListasync(string vendor = null, int id = -1)
        {
            
            if (vendor != null && id != -1)
            {

                List<Item> compareItem = new List<Item>();
                compareItem.Add(new Item()
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
       */
        public  IActionResult AddItem(string vendor , int id)
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "myList") == null)
            {
                List<Item> myList = new List<Item>
                {
                    new Item()
                    {
                        id = id,
                        vendor = vendor
                    }
                };

                SessionHelper.SetObjectAsJson(HttpContext.Session, "myList", myList);
            }
            else
            {
                List<Item> myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "myList");
               
                    myList.Add(new Item()
                    {
                        id = id,
                        vendor = vendor
                    });

                
                SessionHelper.SetObjectAsJson(HttpContext.Session, "myList", myList);
            }
            return RedirectToAction("CompareChart");
        }

        public IActionResult Remove(int id)
        {
            List<Item> myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "myList");
            //int index = isExist(id);
            myList.RemoveAt(id);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "myList", myList);
            return RedirectToAction("CompareChart");
        }
        private bool IsNull()
        {
            List<Item> myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
           
                if (myList!=null)
                {
                    return true;
                }
           
            return false;
        }
        //private int isExist(string id)
        //{
        //    List<Item> myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "myList");
        //    int n = 0;
        //    foreach (var product in myList) n++;
        //        for (int i = 0; i < n; i++)
        //    {
        //        if (myList[i].id.Equals(id))
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}

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
           

             return View("./ProductsList"); ;
        }
        public async Task<IActionResult> SortProductsAsync(string Str)
        {
            using var client = new HttpClient();
            var Url = $"https://comparebazaar-api.herokuapp.com/api/flipkart/mobile/?ordering={Str}";

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
                var Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile";

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


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var currentUserName = User.Identity.Name;
            var loggedInUser = await _userManager.FindByEmailAsync(currentUserName);

            ViewBag.loggedInUser = loggedInUser;

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(IFormCollection userCollection)
        {
            try
            {
                var loggedInUser = await _userManager.FindByNameAsync(User.Identity.Name);

                loggedInUser.FirstName = userCollection["FirstName"];
                loggedInUser.LastName = userCollection["LastName"];
                loggedInUser.Email = userCollection["Email"];
                loggedInUser.UserName = userCollection["Email"];
                loggedInUser.PhoneNumber = userCollection["PhoneNumber"];
                loggedInUser.Address1 = userCollection["Address"];
                loggedInUser.Address2 = userCollection["City"];
             //   loggedInUser.State = userCollection["State"];
               // loggedInUser.PostCode = int.Parse(userCollection["PostCode"]);

                var isSuccess = await _userManager.UpdateAsync(loggedInUser);

                if (isSuccess.Succeeded)
                {
                    _logger.LogInformation("User updated successfully.");
                    return RedirectToAction("EditProfile");
                }

                // If Operation Failes, Redirect to Index
                _logger.LogInformation("User Profile Update Failed");
                return RedirectToAction("Error", "Index");
            }
            catch (System.Exception ex)
            {
                // Log Exception
                _logger.LogError(ex.ToString());
                // return Error Page or Index
                return RedirectToAction("Error", "Index");
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
