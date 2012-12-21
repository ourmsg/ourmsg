using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace IMLibrary3.Data
{
    /// <summary>
    /// ClassOptionData 的摘要说明。
    /// </summary>
    public sealed class OleDbHelper
    {
        /// <summary>
        /// 设置或获取联接字符串
        /// </summary>
        public static string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Record.dll;Persist Security Info=False";//数据库连接字符串 

        private OleDbHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 执行任何SQL语句，返回所影响的行数
        /// </summary>
        /// <param name="SQLStr">SQL命令</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExSQL(string SQLStr)
        {
            try
            {
                OleDbConnection cnn = new OleDbConnection(ConStr);
                OleDbCommand cmd = new OleDbCommand(SQLStr, cnn);
                cnn.Open();
                int i = 0;
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cnn.Close();
                cnn.Dispose();
                return i;
            }
            catch { return 0; }

        }

        /// <summary>
        /// 执行任何SQL语句，返回所影响的行数
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="par"></param>
        /// <param name="SQLStr">SQL命令</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExSQLLengData(object Data, string par, string SQLStr)//
        {
            try
            {
                OleDbConnection cnn = new OleDbConnection(ConStr);
                OleDbCommand cmd = new OleDbCommand(SQLStr, cnn);
                cnn.Open();
                int i = 0;
                cmd.Parameters.Add(par, System.Data.OleDb.OleDbType.Binary);
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cnn.Close();
                cnn.Dispose();
                return i;
            }
            catch { return 0; }

        }

        /// <summary>
        /// 执行任何SQL查询语句，返回所影响的行数
        /// </summary>
        /// <param name="SQLStr">SQL命令</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExSQLR(string SQLStr)
        {
            try
            {
                OleDbConnection cnn = new OleDbConnection(ConStr);
                OleDbCommand cmd = new OleDbCommand(SQLStr, cnn);
                cnn.Open();
                OleDbDataReader dr;
                int i = 0;
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr != null)
                    while (dr.Read())
                    { i++; }
                cmd.Dispose();
                cnn.Close();
                cnn.Dispose();
                return i;
            }
            catch { return 0; }

        }

        /// <summary>
        /// 执行任何SQL查询语句，返回一个字段值
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="SQLStr">SQL命令</param>
        /// <returns>返回一个字段值</returns>
        public static object ExSQLReField(string field, string SQLStr)
        {
            try
            {
                OleDbConnection cnn = new OleDbConnection(ConStr);
                OleDbCommand cmd = new OleDbCommand(SQLStr, cnn);
                cnn.Open();
                OleDbDataReader dr;
                object fieldValue = null;
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr != null && dr.Read())
                { fieldValue = dr[field]; }
                dr.Close();
                cmd.Dispose();
                cnn.Close();
                cnn.Dispose();
                return fieldValue;
            }
            catch { return null; }

        }

        /// <summary>
        /// 执行任何SQL查询语句，返回一个OleDbDataReader
        /// </summary>
        /// <param name="SQLStr">SQL命令</param>
        /// <returns>返回一个OleDbDataReader</returns>
        public static OleDbDataReader ExSQLReDr(string SQLStr)
        {
            try
            {
                OleDbConnection cnn = new OleDbConnection(ConStr);
                OleDbCommand cmd = new OleDbCommand(SQLStr, cnn);
                cnn.Open();
                OleDbDataReader dr;
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch { return null; }
        }

    }
}
