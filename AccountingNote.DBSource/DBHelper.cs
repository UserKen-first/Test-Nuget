using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote.DBSource
{
    class DBHelper
    {
        public static string GetConnectionString()
        {
            string val = ConfigurationManager.ConnectionStrings
                ["DefaultConnection"].ConnectionString;
            return val;
        }
        public static DataTable ReadDataTable(string connStr, string dbCommand, List<SqlParameter> list)
        {
            using (SqlConnection conn = new SqlConnection(connStr))  //連接字串{}是執行範圍，大括號跑完就關掉資料庫，所以為離線存取資料。
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    //comm.Parameters.AddWithValue("@userID", userID); //使用參數化查詢，因已加入參數。

                    comm.Parameters.AddRange(list.ToArray());  //AddRange裡面規定只能裝陣列，故須先轉型成陣列

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public static DataRow ReadDataRow(string connStr, string dbCommand, List<SqlParameter> list)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    comm.Parameters.AddRange(list.ToArray());

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    if (dt.Rows.Count == 0)
                        return null;

                    return dt.Rows[0];
                }
            }
        }

    }
}

