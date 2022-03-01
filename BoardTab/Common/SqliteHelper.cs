using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BoardTab.Common
{
    public class SQLiteHelper
    {
        private static string connectionString = string.Format(@"Data Source={0}", "D:\\DataBase\\BoardDB.db");

        /// <summary>
        /// 适合增删改操作，返回影响条数
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqliteParameter[] parameters)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                using (SqliteCommand comm = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                        comm.CommandText = sql;
                        comm.Parameters.AddRange(parameters);
                        return comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (conn != null && conn.State != ConnectionState.Closed)
                            conn.Close();
                    }

                }
            }
        }

        /// <summary>
        /// 查询操作，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params SqliteParameter[] parameters)
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                using (SqliteCommand comm = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                        comm.CommandText = sql;
                        comm.Parameters.AddRange(parameters);
                        return comm.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (conn != null && conn.State != ConnectionState.Closed)
                            conn.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="sqlText">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static SqliteDataReader ExecuteReader(string sql, params SqliteParameter[] parameters)
        {
            SqliteConnection conn = null;
            try
            {
                //SqlDataReader要求，它读取数据的时候有，它独占它的SqlConnection对象，而且SqlConnection必须是Open状态
                conn = new SqliteConnection(connectionString);//不要释放连接，因为后面还需要连接打开状态
                SqliteCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                //CommandBehavior.CloseConnection当SqlDataReader释放的时候，顺便把SqlConnection对象也释放掉
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                conn.Close();
            }
            return null;
        }


        /// <summary>
        /// Adapter调整，查询操作，返回DataTable
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params SqliteParameter[] parameters)
        {
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, connectionString))
            {
                DataTable dt = new DataTable();
                adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.Fill(dt);
                return dt;
            }
        }

      

    }
}
