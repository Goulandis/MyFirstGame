using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSever.Model
{
    //专门负责数据库Result表的返回结果的服务器端的存储
    class Result
    {
         public Result(int id, int userId,int totalCount,int winCount)
        {
            this.Id = id;
            this.UserId = userId;
            this.TotalCount = totalCount;
            this.WinCount = winCount;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalCount { get; set; }
        public int WinCount { get; set; }
    }
}
