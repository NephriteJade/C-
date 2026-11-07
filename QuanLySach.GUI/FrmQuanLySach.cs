using QuanLySach.BUS;
using QuanLySach.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySach.GUI
{
    public partial class FrmQuanLySach : Form
    {
        private readonly SachBUS _sachBUS = new SachBUS();
        private readonly TheLoaiBUS _theLoaiBUS = new TheLoaiBUS();
        private bool _isClosingConfirmed = false;

        public FrmQuanLySach()
        {
            InitializeComponent();
        }

        private void FrmQuanLySach_Load(object sender, EventArgs e)
        {
            try
            {
                Database.SetInitializer(new DbInitializer());
                using (var db = new QuanLySachDbContext())
                {
                    db.Database.Initialize(force: true);
                }

                // Tự động tạo cấu trúc 6 cột cố định bằng code-behind
                InitDataGridViewColumns();

                LoadTheLoaiToComboBox();
                LoadDanhSachSachToGrid();
                ResetFormState();
            }
            catch (Exception ex)
            {
                string detail = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Lỗi khởi tạo Database: " + detail, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tạo 6 cột chủ động để không phụ thuộc vào Designer
        private void InitDataGridViewColumns()
        {
            dgvSach.Columns.Clear();
            dgvSach.Columns.Add("colMaSach", "Mã sách");
            dgvSach.Columns.Add("colTenSach", "Tên sách");
            dgvSach.Columns.Add("colNgayNhap", "Ngày nhập");
            dgvSach.Columns.Add("colSoLuong", "Số lượng");
            dgvSach.Columns.Add("colDonGia", "Đơn giá");
            dgvSach.Columns.Add("colTheLoai", "Thể loại");
        }

        private void LoadTheLoaiToComboBox()
        {
            try
            {
                var dsTheLoai = _theLoaiBUS.GetAll();
                cboTheLoai.DataSource = dsTheLoai;
                cboTheLoai.DisplayMember = "TenLoai";
                cboTheLoai.ValueMember = "MaLoai";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp thể loại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachSachToGrid()
        {
            try
            {
                dgvSach.Rows.Clear();
                var listSach = _sachBUS.GetAll();
                foreach (var s in listSach)
                {
                    dgvSach.Rows.Add(
                        s.MaSach,
                        s.TenSach,
                        s.NgayNhap.ToString("dd/MM/yyyy"),
                        s.SoLuong,
                        s.DonGia.ToString("N0"),
                        s.TheLoai?.TenLoai ?? ""
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetFormState()
        {
            txtMaSach.Text = "";
            txtTenSach.Text = "";
            dtpNgayNhap.Value = DateTime.Now;
            txtSoLuong.Text = "";
            txtDonGia.Text = "";
            cboTheLoai.SelectedIndex = -1;

            txtMaSach.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            dgvSach.ClearSelection();
            txtMaSach.Focus();
        }

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSach.Rows.Count)
            {
                DataGridViewRow row = dgvSach.Rows[e.RowIndex];
                if (row.Cells[0].Value == null) return;

                txtMaSach.Text = row.Cells[0].Value?.ToString();
                txtTenSach.Text = row.Cells[1].Value?.ToString();

                if (DateTime.TryParse(row.Cells[2].Value?.ToString(), out DateTime dt))
                    dtpNgayNhap.Value = dt;

                txtSoLuong.Text = row.Cells[3].Value?.ToString();
                txtDonGia.Text = row.Cells[4].Value?.ToString().Replace(".", "").Replace(",", "");
                cboTheLoai.Text = row.Cells[5].Value?.ToString();

                txtMaSach.Enabled = false;
                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
        }

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSach_CellClick(sender, e);
        }

        private void btnMoi_Click(object sender, EventArgs e)
        {
            ResetFormState();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong))
                    throw new Exception("Số lượng phải là số nguyên!");
                if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia))
                    throw new Exception("Đơn giá không hợp lệ!");

                var sach = new Sach
                {
                    MaSach = txtMaSach.Text.Trim(),
                    TenSach = txtTenSach.Text.Trim(),
                    NgayNhap = dtpNgayNhap.Value,
                    SoLuong = soLuong,
                    DonGia = donGia,
                    MaLoai = cboTheLoai.SelectedValue?.ToString()
                };

                _sachBUS.ThemSach(sach);
                MessageBox.Show("Thêm mới sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachSachToGrid();
                ResetFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong))
                    throw new Exception("Số lượng phải là số nguyên!");
                if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia))
                    throw new Exception("Đơn giá không hợp lệ!");

                var sach = new Sach
                {
                    MaSach = txtMaSach.Text.Trim(),
                    TenSach = txtTenSach.Text.Trim(),
                    NgayNhap = dtpNgayNhap.Value,
                    SoLuong = soLuong,
                    DonGia = donGia,
                    MaLoai = cboTheLoai.SelectedValue?.ToString()
                };

                _sachBUS.SuaSach(sach);
                MessageBox.Show("Cập nhật thông tin sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachSachToGrid();
                ResetFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa sách này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    _sachBUS.XoaSach(txtMaSach.Text.Trim());
                    MessageBox.Show("Xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSachToGrid();
                    ResetFormState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}