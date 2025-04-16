using DALTWeb.Data;
using DALTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DALTWeb.Repositories
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly ApplicationDbContext _db;


        public SanPhamRepository(ApplicationDbContext db) 
        {
            _db = db;
        }



        public void Delete(int id)
        {
            var sanpham = _db.SanPham.Find(id);
            if(sanpham != null)
                _db.Remove(sanpham);
        }

        public IEnumerable<SanPham> GetAll()
        {
            var sanpham = _db.SanPham.ToList();
            return sanpham;

        }

        public SanPham GetById(int id)
        {
            var sanpham = _db.SanPham.Find(id);
            return sanpham;
        }

        public IEnumerable<SanPham> IncludeBy(string str)
        {
            var sanpham = _db.SanPham.Include(str).ToList();
            return sanpham;
        }

        public void Insert(SanPham sanpham)
        {
            _db.SanPham.Add(sanpham);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(SanPham sanpham)
        {
            _db.Update(sanpham);
        }





    }
}
