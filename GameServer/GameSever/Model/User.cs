using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSever.Model
{
    //专门负责数据库User表的返回结果的服务器端的存储
    class User
    {
        public User(int id, string username, string password)
        {
            this.Id = id;
            this.UserName = username;
            this.Password = password;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
