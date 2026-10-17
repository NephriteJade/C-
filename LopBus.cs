using QuanLySinhVien.Data.DAL;
using QuanLySV.Models;
using System;
using System.Collections.Generic;

namespace QuanLySinhVien.BUL
{
    public class LopBus
    {
        private LopHocDAO lopHocDAO = new LopHocDAO();

        public List<LopHoc> LayDanhSachLop()
        {
            try
            {
                return lopHocDAO.GetAllLopHoc();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách lớp học: " + ex.Message, ex);
            }
        }
    }
}