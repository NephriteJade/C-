using System.Collections.Generic;
using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace QuanLySach.DAL
{
    // BẮT BUỘC: Đăng ký MySQL Configuration cho DbContext
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class QuanLySachDbContext : DbContext
    {
        public QuanLySachDbContext() : base("name=QuanLySachConnection") { }

        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<Sach> Sachs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Ánh xạ chính xác tên bảng trong MySQL Workbench
            modelBuilder.Entity<TheLoai>().ToTable("TheLoai");
            modelBuilder.Entity<Sach>().ToTable("Sach");

            // 2. Sửa lỗi decimal(18,2) của MySQL Provider
            modelBuilder.Entity<Sach>()
                        .Property(s => s.DonGia)
                        .HasColumnType("decimal");
        }
    }

    public class DbInitializer : CreateDatabaseIfNotExists<QuanLySachDbContext>
    {
        protected override void Seed(QuanLySachDbContext context)
        {
            var dsTheLoai = new List<TheLoai>
            {
                new TheLoai { MaLoai = "TH", TenLoai = "Toán học" },
                new TheLoai { MaLoai = "VL", TenLoai = "Vật lý" },
                new TheLoai { MaLoai = "HH", TenLoai = "Hóa học" }
            };
            context.TheLoais.AddRange(dsTheLoai);

            var dsSach = new List<Sach>
            {
                new Sach { MaSach = "S001", TenSach = "Giải tích 1", NgayNhap = System.DateTime.Parse("2026-10-01"), SoLuong = 10, DonGia = 150000, MaLoai = "TH" },
                new Sach { MaSach = "S002", TenSach = "Đại số tuyến tính", NgayNhap = System.DateTime.Parse("2026-10-03"), SoLuong = 20, DonGia = 170000, MaLoai = "TH" },
                new Sach { MaSach = "S003", TenSach = "Vật lý 1", NgayNhap = System.DateTime.Parse("2026-10-02"), SoLuong = 10, DonGia = 120000, MaLoai = "VL" },
                new Sach { MaSach = "S004", TenSach = "Hóa học 1", NgayNhap = System.DateTime.Parse("2026-10-01"), SoLuong = 10, DonGia = 100000, MaLoai = "HH" },
                new Sach { MaSach = "S005", TenSach = "Hóa học 2", NgayNhap = System.DateTime.Parse("2026-10-02"), SoLuong = 15, DonGia = 150000, MaLoai = "HH" },
            };
            context.Sachs.AddRange(dsSach);

            base.Seed(context);
        }
    }
}