using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameSever.Servers;

namespace GameSever
{
    class Program
    {
        static void Main(string[] args)
        {
            //Server server = new Server("172.24.40.42", 6688);//初始化服务器地址和监听端口号
            Server server = new Server("127.0.0.1", 6688);
            server.Start();//打开套接字，开始接收并处理服务请求
            Console.ReadKey();
        }
    }
}
