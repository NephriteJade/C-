using System.Collections.Generic;
using QuanLySach.DAL;

namespace QuanLySach.BUS
{
    public class TheLoaiBUS
    {
        private readonly TheLoaiDAL _theLoaiDAL = new TheLoaiDAL();

        public List<TheLoai> GetAll() => _theLoaiDAL.GetAll();
    }
}
