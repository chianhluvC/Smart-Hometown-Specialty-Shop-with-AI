using DALTWeb.Data;
using DALTWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DALTWeb.Repositories
{
    public class TheLoaiRepository : ITheLoaiRepository
    {

        private readonly ApplicationDbContext _db;

        public TheLoaiRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public void Delete(int id)
        {
            var theloai = _db.TheLoai.Find(id);
            if(theloai != null) 
            {
                _db.Remove(theloai);
            }

        }

        public IEnumerable<TheLoai> GetAll()
        {
            var theloai = _db.TheLoai.ToList();
            return theloai;

        }

        public TheLoai GetById(int id)
        {
            var theloai = _db.TheLoai.Find(id);

            return theloai;
        }

        public void Insert(TheLoai theloai)
        {
            _db.Add(theloai);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public IEnumerable<SelectListItem> select()
        {
            IEnumerable<SelectListItem> dstheloai = _db.TheLoai.Select(

                item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name,

                }
                );

            return dstheloai;
        }

        public void Update(TheLoai theloai)
        {
            _db.Update(theloai);
        }
    }
}
