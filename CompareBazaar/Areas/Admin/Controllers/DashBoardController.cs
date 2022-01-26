using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompareBazaar.Data;
using Microsoft.AspNetCore.Authorization;

namespace CompareBazaar.Areas.Admin.Controllers
{

    [Authorize]
    [Area("Admin")]
    public class DashBoardController : Controller
    {
       

       
        // GET: Admin/DashBoard

        // GET: Admin/DashBoard/Index
        public IActionResult Index()
        {
            return View();
        }

        

      
       
    }

 
}
