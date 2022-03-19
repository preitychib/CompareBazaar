
using CompareBazaar.Areas.Admin.Models;
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
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CompareBazaar.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

       
        //  private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger,UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            // _client = client;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.mobiles = await GetMobiles("amazon");
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Comparelist") != null)
            {
                var myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Comparelist");

                if (myList.Count > 0)
                {
                    ViewBag.mobile = new List<dynamic>();

                    for (int i = 0; i < myList.Count; i++)
                        ViewBag.mobile.Add(await GetItem(myList[i].vendor, "mobile", myList[i].id));
                }
                ViewBag.n = myList.Count;
               
            }
            //  Console.WriteLine(ViewBag.mobiles);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Id,FirstName,LastName,EmailAddress,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }
        public IActionResult FAQs()
        {
            return View();
        }

        
     
       
        public async Task<IActionResult> CompareChartAsync()
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Comparelist") != null)
            {
                var myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Comparelist");

                if (myList.Count > 0)
                {
                    ViewBag.mobile = new List<dynamic>();

                    for (int i = 0; i < myList.Count; i++)
                        ViewBag.mobile.Add(await GetItem(myList[i].vendor, "mobile", myList[i].id));
                }
                ViewBag.n = myList.Count;
              //  return View();
            }
           
           
                return View();
        } 
        public async Task<IActionResult> WishListAsync()
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Wishlist") != null)
            {
                var myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Wishlist");
                ViewBag.mobile = new List<dynamic>();
                if (myList.Count > 0)
                {
                    for (int i = 0; i < myList.Count; i++)
                        ViewBag.mobile.Add(await GetItem(myList[i].vendor, "mobile", myList[i].id));
                }
                 ViewBag.n = myList.Count;
              
                
            }
           
           
                return View();
        }
     
        public  IActionResult AddItem(string vendor , int id, string View,string list)
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, list) == null)
            {
                List<Item> myList = new List<Item>
                {
                    new Item()
                    {
                        id = id,
                        vendor = vendor
                    }
                };

                SessionHelper.SetObjectAsJson(HttpContext.Session, list, myList);
            }
            else
            {
                List<Item> myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, list);
               
                    myList.Add(new Item()
                    {
                        id = id,
                        vendor = vendor
                    });

                
                SessionHelper.SetObjectAsJson(HttpContext.Session, list, myList);
            }


            return RedirectToAction(View);
        }

        public IActionResult Remove(int id, string View,string list)
        {
            List<Item> myList = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, list);
            //int index = isExist(id);
            myList.RemoveAt(id);
            SessionHelper.SetObjectAsJson(HttpContext.Session, list, myList);
            return RedirectToAction(View);
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

        
     
     
        public async Task<IActionResult> ProductsListAsync(string vendor="flipkart", int pageSize = 4, int pageNum = 2,int fVendor=1, string order = "-price", string searchStr = null,string availability=null,int fBrand=-1,int pstart=0,int pend=90000000)
        {
           
            var fmobiles = await GetMobiles(vendor,pageSize,pageNum,fVendor,order,searchStr,availability,fBrand,pstart,pend);
           var amobiles = await GetMobiles("amazon",pageSize,pageNum,fVendor,order,searchStr,availability,fBrand,pstart,pend);
            dynamic mobiles = new ExpandoObject();
            
             ViewBag.mobiles = new List<dynamic>();
            ViewBag.mobiles.Add(fmobiles);
            ViewBag.mobiles.Add(amobiles);
           //Console.WriteLine( ViewBag.mobiles[1].results.Count==null);
           
             
            ViewBag.brands = await GetBrands(vendor);
           
            return View();
           
        }
        private static async Task<object> GetMobiles(string vendor,int pageSize=4,int pageNum=1,int fVendor=1, string order="-price", string searchStr=null,string avlbty=null,int brand=-1,int pstart=0,int pend= 90000000)
        {
            try
            {
                using var client = new HttpClient();
                string Url;
                //Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile/?ordering=-price";
                if (searchStr == null && brand > -1)
                {
                    Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile/?ordering={order}&availability={avlbty}&vendor={fVendor}&page_size={pageSize}&page={pageNum}&price_start={pstart}&price_end={pend}";// brand not added
                   // Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile/";
                }
                else
                {
                    Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile/?search={searchStr}&ordering={order}&vendor={fVendor}&page_size={pageSize}&page={pageNum}&availability={avlbty}&price_start={pstart}&price_end={pend}"; //brand not added
                    //Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/mobile/?search={searchStr}&ordering={order}";
                }
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
           // Product mobile = new Product();
           var mobile = await GetItem(vendor,"mobile",id);
            ViewBag.mobile = mobile;
           // Console.WriteLine(mobile);

            return View();
        }

        private static async Task<object> GetItem(string vendor,string item,int id)
        {
            try
            {
                using var client = new HttpClient();
                var Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/{item}/{id}";

                // return await client.GetAsync(url);
                var Response = await client.GetAsync(Url);
                Response.EnsureSuccessStatusCode();

                var Content = await Response.Content.ReadAsStringAsync();
              //  Product Mobiles = new Product();
               var itemObject =  JsonConvert.DeserializeObject<object>(Content);
                // Mobiles =  JsonConvert.DeserializeObject<Product>(Content);


                // var newMobiles = await _mobileService.GetMobiles();
                return itemObject;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return null;
            }
        }
        private static async Task<object> GetBrands(string vendor)
        {
            try
            {
                using var client = new HttpClient();
                var Url = $"https://comparebazaar-api.herokuapp.com/api/{vendor}/brand";

                // return await client.GetAsync(url);
                var Response = await client.GetAsync(Url);
                Response.EnsureSuccessStatusCode();

                var Content = await Response.Content.ReadAsStringAsync();

                var brands = JsonConvert.DeserializeObject<object>(Content);


                // var newMobiles = await _mobileService.GetMobiles();
                return brands;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return null;
            }
        }

        public async Task<ActionResult> EditProfile()
        {
            var currentUserName = User.Identity.Name;
            var user = await _userManager.FindByEmailAsync(currentUserName);

            ViewBag.user = user;
            
            return View();
        }

       
        [HttpPost]
        public async Task<ActionResult> EditProfile(IFormCollection userEdit)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.FirstName = userEdit["FirstName"];
                user.LastName = userEdit["LastName"];
                user.Email = userEdit["Email"];
                user.UserName = userEdit["Email"];
                user.Address1 = userEdit["Address1"];
                user.Address2 = userEdit["Address2"];
                user.PhoneNumber = userEdit["PhoneNumber"]; ;
                user.PostCode = userEdit["Postcode"]; 

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to update User");
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Index");

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToAction("Error");
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
