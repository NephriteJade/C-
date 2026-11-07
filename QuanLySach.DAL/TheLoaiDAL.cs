using System.Collections.Generic;
using System.Linq;

namespace QuanLySach.DAL
{
    public class TheLoaiDAL
    {
        public List<TheLoai> GetAll()
        {
            using (var db = new QuanLySachDbContext())
            {
                return db.TheLoais.ToList();
            }
        }
    }
}