using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVienApp.Models
{
    public class LopHoc
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public LopHoc() { }
        public LopHoc(string maLop, string tenLop)
        {
            MaLop = maLop;
            TenLop = tenLop;
        }
        public override string ToString() => TenLop;
    }
}
