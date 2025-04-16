using DALTWeb.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DALTWeb.ViewComponents
{
    public class GioHangViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public GioHangViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.cartsum = 0;
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim == null)
            {               
                return View();
            }
            var giohang = _db.GioHang.Where(p => p.ApplicationUserId == claim.Value).ToList();
            if(giohang != null) 
            {
                ViewBag.cartsum = giohang.Count();
            }
            return View();

        }


    }
}
