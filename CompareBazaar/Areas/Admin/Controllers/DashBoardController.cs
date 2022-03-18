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

    //[Authorize(Roles = "Admin")]
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

        
       // [Route("Admin/User/List")]
        public ActionResult UserList(int id)
        {
            var userlist = _userManager.Users.ToList();
            ViewBag.userlist = userlist;
            return View();
        }

       // [Route("Admin/User/Edit/{id}")]
        public async Task<ActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewBag.user = user;

            return View();
        }

       // [Route("Admin/User/Edit/{id}")]
        [HttpPost]
        public async Task<ActionResult> EditUser(string id, [FromForm] UserEdit userEdit)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                user.FirstName = userEdit.FirstName;
                user.LastName = userEdit.LastName;
                user.Email = userEdit.Email;
                user.UserName = userEdit.Email;

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

     //   [Route("Admin/User/Delete/{id}")]
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

       // [Route("Admin/User/Add")]
        [HttpGet]
        public IActionResult UserAdd()
        {
            return View();
        }

      //  [Route("Admin/User/Add")]
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


        //[HttpPost]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.ContactUs.FindAsync(id);
            _context.ContactUs.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      /*  public async Task<IActionResult> ContactDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.ContactUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }
      */
    }



}
