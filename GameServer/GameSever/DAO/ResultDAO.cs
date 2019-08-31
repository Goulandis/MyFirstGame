using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameSever.Model;

namespace GameSever.DAO
{
    //专门负责处理对数据库中Result表的各种操作
    class ResultDAO
    {
        //通过用户ID查找战记信息
        public Result GetResultByUserId(MySqlConnection conn, int userId)
        {
            MySqlDataReader reader = null;//用于存储数据库的查询结果
            try
            {
                //绑定查询命令和数据路链接
                MySqlCommand cmd = new MySqlCommand("select * from result where userid = @userId", conn);
                cmd.Parameters.AddWithValue("userid", userId);//MySql变量的赋值格式
                reader = cmd.ExecuteReader();//向数据库发起查询命令
                if (reader.Read())
                { 
                    int id = reader.GetInt32("id");
                    int totalCount = reader.GetInt32("totalcount");
                    int winCount = reader.GetInt32("wincount");
                    Result result = new Result(id, userId,totalCount,winCount);//转存数据库的查询结果到对应的类中
                    return result;
                }
                else
                {
                    return new Result(-1,userId,0,0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetResultByUserId的时候出现异常:" + e);
            }
            finally
            {
                if (reader != null)
                    reader.Close();//释放MySqlDataReader对象
            }
            return null;
        }

        //对数据库Result表做更新和插入战记操作
        public void UpdateOrAddResult(MySqlConnection conn, Result res)
        {
            try
            {
                MySqlCommand cmd = null;
                if (res.Id <= -1)//如果Result中不存在当前ID，就做插入操作
                {
                    cmd = new MySqlCommand("insert into result set totalcount = @totalcount,wincount = @wincount,userid = @userid",conn);
                }
                else//如果Result表中存在当前ID，则作更新操作
                {
                    cmd = new MySqlCommand("update result set totalcount = @totalcount,wincount = @wincount where userid = @userid",conn);
                }
                //为SQL命令变量赋值
                cmd.Parameters.AddWithValue("totalcount",res.TotalCount);
                cmd.Parameters.AddWithValue("wincount",res.WinCount);
                cmd.Parameters.AddWithValue("userid", res.UserId);
                cmd.ExecuteNonQuery();//向数据库发送非查询命令
                if (res.Id <= -1)//由于Result表的ID是自增的，所以当进行的是插入操作时，反馈Result的ID到服务器
                {
                    Result tempResult = GetResultByUserId(conn, res.UserId);
                    res.Id = tempResult.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在UpdateRoAddResult时出现异常"+e);
                throw;
            }
        }
    }
}
