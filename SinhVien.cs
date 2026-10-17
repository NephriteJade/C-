using System;

namespace QuanLySV.Models
{
    public class SinhVien
    {
        public string MaSV { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; } = DateTime.Now;
        public string? GioiTinh { get; set; }
        public string? MaLop { get; set; }
        public double Diem { get; set; }
        public string? DienThoai { get; set; }
        public string? Email { get; set; }
        public string? TrangThai { get; set; }

        public SinhVien() { }

        public SinhVien(string maSV, string hoTen, DateTime ngaySinh, string? gioiTinh, string? maLop, double diem, string? dienThoai, string? email, string? trangThai)
        {
            MaSV = maSV;
            HoTen = hoTen;
            NgaySinh = ngaySinh;
            GioiTinh = gioiTinh;
            MaLop = maLop;
            Diem = diem;
            DienThoai = dienThoai;
            Email = email;
            TrangThai = trangThai;
        }
    }
}