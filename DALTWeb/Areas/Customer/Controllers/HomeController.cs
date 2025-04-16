using DALTWeb.Data;
using DALTWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace DALTWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(string tk, string dm1, string dm2)
        {
            IEnumerable<SanPham> sanpham = _db.SanPham.Include(sp => sp.TheLoai).ToList();
            ViewBag.TitleTK = "Tất cả đặc sản";
            if (!string.IsNullOrEmpty(tk))
            {
                sanpham = _db.SanPham.Where(p => p.Name.Contains(tk)).ToList();
                ViewBag.TitleTK = "Đặc sản " + tk;
            }
            else if (!string.IsNullOrEmpty(dm1))
            {
                var loai = _db.TheLoai.FirstOrDefault(p => p.Name == dm1);
                sanpham = _db.SanPham.Where(p => p.TheLoaiId == loai.Id).ToList();
                ViewBag.TitleTK = dm1 + " các loại";
            }
            else if (!string.IsNullOrEmpty(dm2))
            {
                var loai = _db.TheLoai.FirstOrDefault(p => p.Name == dm2);
                sanpham = _db.SanPham.Where(p => p.TheLoaiId == loai.Id).ToList();
                ViewBag.TitleTK = dm2 + " các loại";
            }

            ViewBag.SP = sanpham;
            IEnumerable<TheLoai> theloai = _db.TheLoai.ToList();
            ViewBag.TheLoai = theloai;
            return View(sanpham);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Details(int sanphamId)
        {
            GioHang giohang = new GioHang()
            {
                SanPhamId = sanphamId,
                SanPham = _db.SanPham.Include("TheLoai").FirstOrDefault(sp => sp.Id == sanphamId),
                Quantity = 1
            };

           
            return View(giohang);
        }

        [HttpPost]
        [Authorize]

        public IActionResult Details(GioHang giohang )
        {
            
            var identity = (ClaimsIdentity)User.Identity; 
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            giohang.ApplicationUserId = claim.Value;
                      
            var giohangdb = _db.GioHang.FirstOrDefault(sp => sp.SanPhamId == giohang.SanPhamId && sp.ApplicationUserId == giohang.ApplicationUserId);
            if (giohangdb == null) 
            {
                _db.GioHang.Add(giohang);
            }
            else
            {
                giohangdb.Quantity += giohang.Quantity; 
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult DiaChi()
        {

            return View();
        }

        [Authorize]
        public IActionResult ThemVaoGio(int sanphamId)
        {
            SanPham? sanpham = _db.SanPham.FirstOrDefault(p => p.Id == sanphamId);
            if (sanpham == null)
                return BadRequest("San pham khong ton tai");

            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            
            var giohang = _db.GioHang.FirstOrDefault(p => p.ApplicationUserId == claim.Value && p.SanPhamId == sanphamId);
            
            if(giohang == null)
            {
                giohang = new GioHang()
                {
                    SanPhamId = sanphamId,
                    Quantity = 1,
                    ProductPrice = sanpham.Price,
                    ApplicationUserId = claim.Value
                };
                _db.GioHang.Add(giohang);
            }
            else
            {
                giohang.Quantity++;
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult PageNotFound()
        {


            return View();
        }

    }
}