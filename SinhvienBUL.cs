using QuanLySinhVien.Data.DAL;
using QuanLySV.Models;
using System;
using System.Collections.Generic;

namespace QuanLySinhVien.BUL
{
    public class SinhVienBUL
    {
        private SinhVienDAO svDAO = new SinhVienDAO();

        public List<SinhVien> LayTatCaSinhVien()
        {
            try
            {
                return svDAO.GetAllSinhVien();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sinh viên: " + ex.Message, ex);
            }
        }

        public SinhVien TimTheoMa(string maSV)
        {
            try
            {
                return svDAO.GetSinhVienByMaSV(maSV);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm sinh viên theo mã: " + ex.Message, ex);
            }
        }

        public List<SinhVien> TimKiemTheoLop(string maLop)
        {
            try
            {
                if (string.IsNullOrEmpty(maLop))
                {
                    return svDAO.GetAllSinhVien();
                }
                return svDAO.GetSinhViensByMaLop(maLop);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sinh viên theo lớp: " + ex.Message, ex);
            }
        }

        public void ThemSinhVien(SinhVien sv)
        {
            try
            {
                var existing = svDAO.GetSinhVienByMaSV(sv.MaSV);
                if (existing != null)
                {
                    throw new Exception("Mã sinh viên đã tồn tại trong hệ thống!");
                }
                svDAO.AddSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sinh viên: " + ex.Message, ex);
            }
        }

        public void CapNhatSinhVien(SinhVien sv)
        {
            try
            {
                svDAO.UpdateSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sinh viên: " + ex.Message, ex);
            }
        }

        public void XoaSinhVien(string maSV)
        {
            try
            {
                svDAO.DeleteSinhVien(maSV);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sinh viên: " + ex.Message, ex);
            }
        }
    }
}