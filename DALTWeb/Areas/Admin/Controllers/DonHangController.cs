using DALTWeb.Data;
using DALTWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DALTWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DonHangController : Controller
	{
		private readonly ApplicationDbContext _db;

		public DonHangController(ApplicationDbContext db) 
		{
			_db = db;
		}


		public IActionResult Index()
		{
			IEnumerable<HoaDon> hoadon = _db.HoaDon.Include("ApplicationUser").ToList();
			return View(hoadon);
		}

		public IActionResult ChiTiet(int iddh)
		{
			var donhang = _db.HoaDon.FirstOrDefault(p => p.Id == iddh);
			ViewBag.MaHoaDon = donhang.Id;
			ViewBag.TenKH = donhang.Name;
			ViewBag.TongTien = donhang.Total;
			var ctdonhang = _db.ChiTietHoaDon.Include("HoaDon").Include("SanPham").Where(p => p.HoaDonId == iddh).ToList();
			
			return View(ctdonhang);
		}

	}
}
