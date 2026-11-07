using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace QuanLySach.DAL
{
    public class SachDAL
    {
        public List<Sach> GetAll()
        {
            using (var db = new QuanLySachDbContext())
            {
                return db.Sachs.Include(s => s.TheLoai).ToList();
            }
        }

        public Sach GetById(string maSach)
        {
            using (var db = new QuanLySachDbContext())
            {
                return db.Sachs.FirstOrDefault(s => s.MaSach == maSach);
            }
        }

        public void Add(Sach sach)
        {
            using (var db = new QuanLySachDbContext())
            {
                db.Sachs.Add(sach);
                db.SaveChanges();
            }
        }

        public void Update(Sach sach)
        {
            using (var db = new QuanLySachDbContext())
            {
                var existing = db.Sachs.FirstOrDefault(s => s.MaSach == sach.MaSach);
                if (existing != null)
                {
                    existing.TenSach = sach.TenSach;
                    existing.NgayNhap = sach.NgayNhap;
                    existing.SoLuong = sach.SoLuong;
                    existing.DonGia = sach.DonGia;
                    existing.MaLoai = sach.MaLoai;
                    db.SaveChanges();
                }
            }
        }

        public void Delete(string maSach)
        {
            using (var db = new QuanLySachDbContext())
            {
                var existing = db.Sachs.FirstOrDefault(s => s.MaSach == maSach);
                if (existing != null)
                {
                    db.Sachs.Remove(existing);
                    db.SaveChanges();
                }
            }
        }
    }
}