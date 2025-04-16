using DALTWeb.Data;
using DALTWeb.Models;
using DALTWeb.Services;
using DALTWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Policy;

namespace DALTWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class GioHangController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IVnPayService _vnPayservice;
		public GioHangController(ApplicationDbContext db, IVnPayService vnPayService)
		{
			_db = db;
			_vnPayservice = vnPayService;
		}

		[Authorize]
		public IActionResult Index()
		{
			var identity = (ClaimsIdentity)User.Identity;
			var claim = identity.FindFirst(ClaimTypes.NameIdentifier);


			GioHangViewModel giohang = new GioHangViewModel()
			{
				DsGioHang = _db.GioHang.Include("SanPham").Where(g => g.ApplicationUserId == claim.Value).ToList(),
				HoaDon = new HoaDon()
			};

			foreach(var item in giohang.DsGioHang)
			{
				item.ProductPrice = item.Quantity * item.SanPham.Price;
				giohang.HoaDon.Total += item.ProductPrice;
			}

			return View(giohang);
		}

		public IActionResult Tang(int giohangid)
		{
			var giohang = _db.GioHang.FirstOrDefault(p => p.Id == giohangid);
			giohang.Quantity++;
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult Giam(int giohangid)
		{
			var giohang = _db.GioHang.FirstOrDefault(p => p.Id == giohangid);
			giohang.Quantity--;
			if(giohang.Quantity==0)
			{
				_db.GioHang.Remove(giohang);
			}
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult Xoa(int giohangid)
		{
			var giohang = _db.GioHang.FirstOrDefault(p => p.Id == giohangid);
			_db.GioHang.Remove(giohang);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}


		public IActionResult ThanhToan()
		{
			var identity = (ClaimsIdentity)User.Identity;
			var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

			GioHangViewModel giohang = new GioHangViewModel()
			{
				DsGioHang = _db.GioHang.Include("SanPham").Where(p => p.ApplicationUserId == claim.Value).ToList(),
				HoaDon = new HoaDon()
			};

			giohang.HoaDon.ApplicationUser = _db.ApplicationUser.FirstOrDefault(user => user.Id == claim.Value);

			giohang.HoaDon.Name = giohang.HoaDon.ApplicationUser.Name;
			giohang.HoaDon.Address = giohang.HoaDon.ApplicationUser.Address;
			giohang.HoaDon.PhoneNumber = giohang.HoaDon.PhoneNumber;


			foreach(var item in giohang.DsGioHang)
			{
				item.ProductPrice = item.Quantity * item.SanPham.Price;
				giohang.HoaDon.Total += item.ProductPrice;
			}


			return View(giohang);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		
		public IActionResult ThanhToan(GioHangViewModel giohang, string payment )
		{
			
			var identity = (ClaimsIdentity)User.Identity;
			var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

			giohang.DsGioHang = _db.GioHang.Include("SanPham").Where(p => p.ApplicationUserId == claim.Value).ToList();
			giohang.HoaDon.ApplicationUserId = claim.Value;
			giohang.HoaDon.OrderDate = DateTime.Now;
			giohang.HoaDon.OrderStatus = "Đang xác nhận";
			foreach (var item in giohang.DsGioHang)
			{
				item.ProductPrice = item.Quantity * item.SanPham.Price;
				giohang.HoaDon.Total += item.ProductPrice;
			}
			_db.HoaDon.Add(giohang.HoaDon);
			_db.SaveChanges();



			foreach (var item in giohang.DsGioHang)
			{
				ChiTietHoaDon chitiethoadon = new ChiTietHoaDon()
				{
					SanPhamId = item.SanPhamId,
					HoaDonId = giohang.HoaDon.Id,
					ProductPrice = item.ProductPrice,
					Quantity = item.Quantity
				};

				_db.ChiTietHoaDon.Add(chitiethoadon);
				_db.SaveChanges();

			}
			var hoadon = _db.HoaDon.FirstOrDefault(p => p.Id == giohang.HoaDon.Id);
			_db.GioHang.RemoveRange(giohang.DsGioHang);
			_db.SaveChanges();

			if (payment == "Thanh Toan VNPay")
			{
				var vnPayModel = new VnPaymentRequestModel
				{
					Amount = hoadon.Total*100,
					CreateDate = DateTime.Now,
					Description = $"{hoadon.Id}",
					FullName = hoadon.Name,
					OrderId = hoadon.Id

				};

				return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
			}
			return View("XacNhan", hoadon);
		}

		public IActionResult XacNhan()
		{



			return View();
		}

		[Authorize]
		public IActionResult PaymentFail()
		{
			return View();
		}

		public IActionResult PaymentSuccess()
		{
			return View();
		}




		[Authorize]
		public IActionResult PaymentCallBack()
		{
			var response = _vnPayservice.PaymentExecute(Request.Query);
			if(response == null || response.VnPayResponseCode != "00")
			{
				var hoadon = _db.HoaDon.OrderBy(p => p.Id).LastOrDefault();
				hoadon.OrderStatus = "Loi thanh toan VNPay";
				_db.SaveChanges();
				TempData["Message"] = $"Loi thanh toan VNPay: {response.VnPayResponseCode}";
				return RedirectToAction("PaymentFail");
			}

			int orderid = int.Parse(response.OrderDescription);
			var order = _db.HoaDon.FirstOrDefault(p => p.Id == orderid);
			order.OrderStatus = "Da thanh toan VNPay";
			_db.SaveChanges();
			TempData["Message"] = $"Thanh toan VNPay thanh cong";
			return RedirectToAction("PaymentSuccess");
		}



	}
}
