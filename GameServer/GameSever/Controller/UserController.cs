using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameSever.Servers;
using GameSever.DAO;
using GameSever.Model;

namespace GameSever.Controller
{
    //RequestCode.User映射的Controller类
    class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();//数据库中User表映射的DAO类对象
        private ResultDAO resultDAO = new ResultDAO();//数据中Result表映射的DAO类对象

        public UserController()
        {
            requestCode = RequestCode.User;//建立映射关系
        }

        //ActionCode.Login对应的处理函数
        public string Login(string data,Client client, Server servert)
        {
            string[] strs = data.Split(',');//分割客户端传入的用户信息
            string username = strs[0];
            string password = strs[1];
            //向数据库的User表发起连接并核查用户名与密码
            User user = userDAO.VerifyUser(client.MySqlConn, username,password);
            if (user == null)
            {
               //用户不存在则登陆失败
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                //向数据库的Result表发起连接并获取战记结果
                Result res = resultDAO.GetResultByUserId(client.MySqlConn, user.Id);
                client.SetUserData(user,res);//在服务器端保存当前客户端信息
                return string.Format("{0},{1},{2},{3}", ((int) ReturnCode.Success).ToString(), user.UserName, res.TotalCount,
                    res.WinCount);
            }
        }

        //对应ActionCode.Register的处理函数
        public string Register(string data, Client client, Server servert)
        {
            string[] strs = data.Split(',');
            string username = strs[0];
            string password = strs[1];
            //向数据库的User表发起连接并核查名称是否已存在
            bool isUserExist = userDAO.GetUserByUsername(client.MySqlConn, username);
            if (isUserExist)
            {   
                //如名称已被注册则注册失败            
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                //向数据库User表建立连接并新增当前用户到User表中存储
                userDAO.AddUser(client.MySqlConn, username, password);
                return ((int)ReturnCode.Success).ToString();
            }
        }
    }
}
