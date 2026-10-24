using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace QuanLySinhVien.Data.DAL
{
    public class DataHelper
    {
        // Chuỗi kết nối tới MySQL với database universityDB
        private readonly string connectionString = @"Server=localhost; Port=3306; Database=universityDB; Uid=root; Pwd=Deptraithi<3;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public DataTable GetDataTable(string sql, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection con = GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi truy vấn cơ sở dữ liệu MySQL: " + ex.Message, ex);
            }
        }

        public int ExecuteNonQuery(string sql, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection con = GetConnection())
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thực thi dữ liệu MySQL: " + ex.Message, ex);
            }
        }
    }
}