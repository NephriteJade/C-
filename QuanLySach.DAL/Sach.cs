using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySach.DAL
{
    [Table("Sach")]
    public class Sach
    {
        [Key]
        [StringLength(20)]
        public string MaSach { get; set; }

        [Required]
        [StringLength(150)]
        public string TenSach { get; set; }

        public DateTime NgayNhap { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "decimal")]
        public decimal DonGia { get; set; }

        [Required]
        [StringLength(10)]
        public string MaLoai { get; set; }

        [ForeignKey("MaLoai")]
        public virtual TheLoai TheLoai { get; set; }
    }
}