using DALTWeb.Models;

namespace DALTWeb.Repositories
{
    public interface ISanPhamRepository
    {
        IEnumerable<SanPham> GetAll();
        SanPham GetById(int id);
        void Insert(SanPham sanpham);
        void Update(SanPham sanpham);

        void Delete(int id);

        IEnumerable<SanPham> IncludeBy(string str);
        void Save();

    }
}
