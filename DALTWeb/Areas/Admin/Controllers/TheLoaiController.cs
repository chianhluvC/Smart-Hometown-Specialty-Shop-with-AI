using Microsoft.AspNetCore.Mvc;
using DALTWeb.Repositories;
using DALTWeb.Models;
namespace DALTWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class TheLoaiController : Controller
	{
		public readonly ITheLoaiRepository _theloaiRepository;


		public TheLoaiController(ITheLoaiRepository theloaiRepository)
		{
			_theloaiRepository = theloaiRepository;
		}



		public IActionResult Index()
		{
			var theloai = _theloaiRepository.GetAll();
			ViewBag.TheLoai = theloai;
			
			return View();
		}

		[HttpGet]

		public IActionResult Create() 
		{
			return View();
		}

		[HttpPost]

		public IActionResult Create(TheLoai theloai) 
		{
			if(ModelState.IsValid)
			{
				_theloaiRepository.Insert(theloai);
				_theloaiRepository.Save();
				return RedirectToAction("Index");
			}

			return View();


		}


		[HttpGet]

		public IActionResult Edit(int id)
		{
			if(id==0)
			{
				return NotFound();
			}

			var theloai = _theloaiRepository.GetById(id);
			return View(theloai);

		}

		[HttpPost]

		public IActionResult Edit(TheLoai theloai) 
		{
			if(ModelState.IsValid) 
			{
				_theloaiRepository.Update(theloai);
				_theloaiRepository.Save();
				return RedirectToAction("Index");
			}
			return View();
		}




		public IActionResult Delete(int id) 
		{
			if (id == 0)
				return NotFound();
			else
			{
				_theloaiRepository.Delete(id);
				_theloaiRepository.Save();
			}

			return RedirectToAction("Index");
		}

		

		public IActionResult detail(int id)
		{
			if (id == 0)
			{
				return NotFound();
			}

			var theloai = _theloaiRepository.GetById(id);


			return View(theloai);
		}

	}
}
