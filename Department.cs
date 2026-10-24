using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySV.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("dept_id")]
        public int DeptId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("dept_code")]
        public string DeptCode { get; set; }

        [Required]
        [StringLength(255)]
        [Column("dept_name")]
        public string DeptName { get; set; }

        [StringLength(100)]
        [Column("location")]
        public string Location { get; set; }

        [StringLength(20)]
        [Column("phone")]
        public string Phone { get; set; }

        [Column("established")]
        public int? Established { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relationship: 1 Khoa có nhiều Sinh viên (Navigation Property)
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}