using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GameSever.Tool
{
    //专门负责与数据库建立连接
    public class ConnHelper
    {
        public const string CONNECTSTRING = "datasource = 127.0.0.1;port = 3306;database = gameserver;user = root;pwd = root";
        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTSTRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch(Exception e)
            {
                Console.WriteLine("连接数据库的时候实现异常：" + e);
                return null;
            }
        }

        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
            }
            else
            {
                Console.WriteLine("MySqlConnection不能为空");
            }
        }
    }
}
