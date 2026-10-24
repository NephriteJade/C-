using System;

namespace QuanLySV.Models
{
    public class SinhVien
    {
        public string MaSV { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string GioiTinh { get; set; } = "Nam";
        public DateTime NgaySinh { get; set; } = DateTime.Now;
        public string DienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MaLop { get; set; } = string.Empty;
        public double Diem { get; set; } = 0.0;
        public string TrangThai { get; set; } = "Đang học";
    }
}