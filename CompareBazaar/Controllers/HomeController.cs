﻿using CompareBazaar.Models;
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

        public async Task<IActionResult> Index()
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

        public IActionResult About()
        {
            return View();
        }

        public IActionResult CompareChart()
        {
            return View();
        }

        public IActionResult ProductsList()
        {
            return View();
        }
        public IActionResult ProductDetails()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
