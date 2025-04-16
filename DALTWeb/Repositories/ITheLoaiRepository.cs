using DALTWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DALTWeb.Repositories
{
    public interface ITheLoaiRepository
    {
        IEnumerable<TheLoai> GetAll();
        TheLoai GetById(int id);

        void Insert(TheLoai theloai);
        void Update(TheLoai theloai);
        void Delete(int id);

        IEnumerable<SelectListItem> select();
        void Save();

    }
}
