using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using QuanLySV.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace QuanLySinhVien.Data.DAL
{
    public class LopHocDAO
    {
        private readonly DataHelper dataHelper = new DataHelper();

        public List<LopHoc> GetAllLopHoc()
        {
            try
            {
                List<LopHoc> dsLop = new List<LopHoc>();
                string sql = "SELECT MaLop, TenLop FROM LopHoc";
                DataTable dt = dataHelper.GetDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    LopHoc lh = new LopHoc
                    {
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        TenLop = dr["TenLop"]?.ToString() ?? string.Empty
                    };
                    dsLop.Add(lh);
                }
                return dsLop;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [GetAllLopHoc]: " + ex.Message, ex);
            }
        }

        public LopHoc? GetLopHocByMaLop(string maLop)
        {
            try
            {
                string sql = "SELECT MaLop, TenLop FROM LopHoc WHERE MaLop = @MaLop";
                MySqlParameter[] parameters = { new MySqlParameter("@MaLop", maLop) };
                DataTable dt = dataHelper.GetDataTable(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    return new LopHoc
                    {
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        TenLop = dr["TenLop"]?.ToString() ?? string.Empty
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [GetLopHocByMaLop]: " + ex.Message, ex);
            }
        }

        public void AddLopHoc(LopHoc lh)
        {
            try
            {
                string sql = "INSERT INTO LopHoc (MaLop, TenLop) VALUES (@MaLop, @TenLop)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@MaLop", lh.MaLop),
                    new MySqlParameter("@TenLop", lh.TenLop)
                };
                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [AddLopHoc]: " + ex.Message, ex);
            }
        }

        public void UpdateLopHoc(LopHoc lh)
        {
            try
            {
                string sql = "UPDATE LopHoc SET TenLop = @TenLop WHERE MaLop = @MaLop";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@MaLop", lh.MaLop),
                    new MySqlParameter("@TenLop", lh.TenLop)
                };
                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [UpdateLopHoc]: " + ex.Message, ex);
            }
        }

        public void DeleteLopHoc(string maLop)
        {
            try
            {
                string sql = "DELETE FROM LopHoc WHERE MaLop = @MaLop";
                MySqlParameter[] parameters = { new MySqlParameter("@MaLop", maLop) };
                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error [DeleteLopHoc]: " + ex.Message, ex);
            }
        }
    }
}