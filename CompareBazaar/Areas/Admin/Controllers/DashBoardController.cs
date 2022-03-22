using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompareBazaar.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using CompareBazaar.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;

namespace CompareBazaar.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class DashBoardController : Controller
    {



        private readonly ILogger<DashBoardController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public DashBoardController(ILogger<DashBoardController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }
       
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PopularProducts()
        {
            return View(await _context.PopularProducts.ToListAsync());
        }

        

        public async Task<IActionResult> DeletePopularProduct(string id)
        {
            var popularProducts = await _context.PopularProducts.FindAsync(id);
            _context.PopularProducts.Remove(popularProducts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        public ActionResult UserList(int id)
        {
            var userlist = _userManager.Users.ToList();
            ViewBag.userlist = userlist;
            return View();
        }

      
        public async Task<ActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewBag.user = user;

            return View();
        }

       
        [HttpPost]
        public async Task<ActionResult> EditUser(string id,IFormCollection userEdit)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

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

                return RedirectToAction("UserList");

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToAction("Error");
            }
        }

     
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to delete User");
                    return RedirectToAction("Error");
                }

                return RedirectToAction("UserList");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToAction("Error");
            }

        }

      
        [HttpGet]
        public IActionResult UserAdd()
        {
            return View();
        }

     
        [HttpPost]
        public async Task<ActionResult> UserAdd(IFormCollection collection)
        {
            try
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = collection["Email"],
                    UserName = collection["Email"],
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"]
                };

                var result = await _userManager.CreateAsync(user, collection["Password"]);

                if (!result.Succeeded)
                {
                    _logger.LogError("User creation failed");
                    return RedirectToAction("Error");
                }
                return RedirectToAction("UserList");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToAction("Error");
            }

        }

        public async Task<IActionResult> ContactList()
        {
            return View(await _context.ContactUs.ToListAsync());
        }


        
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.ContactUs.FindAsync(id);
            _context.ContactUs.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    
    }



}
