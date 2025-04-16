using DALTWeb.Data;
using DALTWeb.Models;
using DALTWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DALTWeb.Areas.Admin.Controllers
{
	[Area("Admin")]


	public class SanPhamController : Controller
	{
		private readonly ISanPhamRepository _sanphamRepository;
		private readonly ITheLoaiRepository _theloaiRepository;
		public SanPhamController(ISanPhamRepository sanphamrepository, ITheLoaiRepository theloairepository)
		{
			_sanphamRepository = sanphamrepository;
			_theloaiRepository = theloairepository;
		}
		public IActionResult Index()
		{
			var sanpham = _sanphamRepository.IncludeBy("TheLoai").ToList();
			ViewBag.sp = sanpham;
			return View();
		}

		[HttpGet]

		public IActionResult Upsert(int id)
		{
			SanPham sanpham = new SanPham();

			IEnumerable<SelectListItem> dstheloai = _theloaiRepository.select();

			ViewBag.DSTheLoai = dstheloai;

			if(id==0)
			{
				return View(sanpham);
			}
			else
			{
				sanpham = _sanphamRepository.IncludeBy("TheLoai").FirstOrDefault(sp => sp.Id == id);
				return View(sanpham);
			}




		}

		[HttpPost]

		public IActionResult Upsert(SanPham sanpham)
		{
			if(ModelState.IsValid)
			{
				
					if(sanpham.Id==0)
					{
						_sanphamRepository.Insert(sanpham);
					}
					else
					{
					_sanphamRepository.Update(sanpham);
					}
				_sanphamRepository.Save();
				return RedirectToAction("Index");
			}


			return View();
		}





		public IActionResult Delete(int id) 
		{
			if(id==0)
			{
				return NotFound();
			}
			_sanphamRepository.Delete(id);
			_sanphamRepository.Save();
			return RedirectToAction("Index");
		}

		public IActionResult detail(int id)
		{
			if (id == 0)
			{
				return NotFound();
			}

			var sanpham = _sanphamRepository.GetById(id);


			return View(sanpham);
		}
	}
}
