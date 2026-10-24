using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using QuanLySV.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace QuanLySinhVien.Data.DAL
{
    public class SinhVienDAO
    {
        private readonly DataHelper dataHelper = new DataHelper();

        public List<SinhVien> GetAllSinhVien()
        {
            try
            {
                List<SinhVien> dsSV = new List<SinhVien>();
                string sql = "SELECT MaSV, HoTen, GioiTinh, NgaySinh, SoDienThoai, MaLop, Diem, Email FROM SinhVien";
                DataTable dt = dataHelper.GetDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    dsSV.Add(MapDataRowToSinhVien(dr));
                }
                return dsSV;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [GetAllSinhVien]: " + ex.Message, ex);
            }
        }

        public SinhVien? GetSinhVienByMaSV(string maSV)
        {
            try
            {
                string sql = "SELECT MaSV, HoTen, GioiTinh, NgaySinh, SoDienThoai, MaLop, Diem, Email FROM SinhVien WHERE MaSV = @MaSV";
                MySqlParameter[] parameters = { new MySqlParameter("@MaSV", maSV) };
                DataTable dt = dataHelper.GetDataTable(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    return MapDataRowToSinhVien(dt.Rows[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [GetSinhVienByMaSV]: " + ex.Message, ex);
            }
        }

        public List<SinhVien> GetSinhViensByMaLop(string maLop)
        {
            try
            {
                List<SinhVien> dsSV = new List<SinhVien>();
                string sql = "SELECT MaSV, HoTen, GioiTinh, NgaySinh, SoDienThoai, MaLop, Diem, Email FROM SinhVien WHERE MaLop = @MaLop";
                MySqlParameter[] parameters = { new MySqlParameter("@MaLop", maLop) };
                DataTable dt = dataHelper.GetDataTable(sql, parameters);

                foreach (DataRow dr in dt.Rows)
                {
                    dsSV.Add(MapDataRowToSinhVien(dr));
                }
                return dsSV;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [GetSinhViensByMaLop]: " + ex.Message, ex);
            }
        }

        public void AddSinhVien(SinhVien sv)
        {
            try
            {
                string sql = @"INSERT INTO SinhVien (MaSV, MaLop, HoTen, GioiTinh, NgaySinh, Diem, SoDienThoai, Email) 
                               VALUES (@MaSV, @MaLop, @HoTen, @GioiTinh, @NgaySinh, @Diem, @SoDienThoai, @Email)";
                MySqlParameter[] parameters = GetSqlParameters(sv);
                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [AddSinhVien]: " + ex.Message, ex);
            }
        }

        public void UpdateSinhVien(SinhVien sv)
        {
            try
            {
                string sql = @"UPDATE SinhVien 
                               SET HoTen = @HoTen, MaLop = @MaLop, GioiTinh = @GioiTinh, 
                                   NgaySinh = @NgaySinh, Diem = @Diem, SoDienThoai = @SoDienThoai, Email = @Email 
                               WHERE MaSV = @MaSV";
                MySqlParameter[] parameters = GetSqlParameters(sv);
                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [UpdateSinhVien]: " + ex.Message, ex);
            }
        }

        public void DeleteSinhVien(string maSV)
        {
            try
            {
                string sql = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
                MySqlParameter[] parameters = { new MySqlParameter("@MaSV", maSV) };
                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [DeleteSinhVien]: " + ex.Message, ex);
            }
        }

        private SinhVien MapDataRowToSinhVien(DataRow dr)
        {
            return new SinhVien
            {
                MaSV = dr["MaSV"]?.ToString() ?? string.Empty,
                HoTen = dr["HoTen"]?.ToString() ?? string.Empty,
                GioiTinh = dr["GioiTinh"]?.ToString() ?? string.Empty,
                NgaySinh = dr["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(dr["NgaySinh"]) : DateTime.MinValue,
                DienThoai = dr["SoDienThoai"]?.ToString() ?? string.Empty,
                MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                Diem = dr["Diem"] != DBNull.Value ? Convert.ToDouble(dr["Diem"]) : 0,
                Email = dr["Email"]?.ToString() ?? string.Empty
            };
        }

        private MySqlParameter[] GetSqlParameters(SinhVien sv)
        {
            return new MySqlParameter[]
            {
                new MySqlParameter("@MaSV", (object?)sv.MaSV ?? DBNull.Value),
                new MySqlParameter("@MaLop", (object?)sv.MaLop ?? DBNull.Value),
                new MySqlParameter("@HoTen", (object?)sv.HoTen ?? DBNull.Value),
                new MySqlParameter("@GioiTinh", (object?)sv.GioiTinh ?? DBNull.Value),
                new MySqlParameter("@NgaySinh", sv.NgaySinh),
                new MySqlParameter("@Diem", sv.Diem),
                new MySqlParameter("@SoDienThoai", (object?)sv.DienThoai ?? DBNull.Value),
                new MySqlParameter("@Email", (object?)sv.Email ?? DBNull.Value)
            };
        }
    }
}