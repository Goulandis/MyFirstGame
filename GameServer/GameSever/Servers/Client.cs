using System;
using System.Net.Sockets;
using Common;
using GameSever.DAO;
using GameSever.Model;
using MySql.Data.MySqlClient;
using GameSever.Tool;

namespace GameSever.Servers
{
    //一个Client对象对应一个客户端
    class Client
    {
        private Socket clientSocket;//存储服务器接收的客户端套接字，一个Client对象连接一个客户端的套接字
        private Server server;//存储服务器的主套接字
        private Message message = new Message();//存储套接字接收到的数据
        private MySqlConnection mySqlConn;//存储Client到数据库的链接，由Client对象与数据库对接
        private ResultDAO resultDao = new ResultDAO();

        private User user;//存储当前客户端的用户信息
        private Result result;//存储当前客户端的战记
        private Room room;//存储当前客户端所在的房间
        private int hp = 200;//有Client对象存储对应客户端玩家的血量值

        public MySqlConnection MySqlConn
        {
            get { return mySqlConn; }
        }

        public Room Room
        {
            set { room = value; }
            get { return room; }
        }

        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public void SetUserData(User user, Result result)
        {
            this.user = user;
            this.result = result;
        }

        //获取用户信息
        public string GetUserData()
        {
            return user.Id+","+ user.UserName + "," + result.TotalCount + "," + result.WinCount;
        }

        public Client() { }
        public Client(Socket clientSocket,Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mySqlConn = ConnHelper.Connect();//建立Client到数据库的连接
        }

        //接受客户端套接字的数据
        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                Console.WriteLine("一个客户端已断开");
                return;
            }
            clientSocket.BeginReceive(message.AcceptByte, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }

        //clientSocket.BeginReceive的回调函数，当接收到数据时回调
        void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)
                    return;
                int count = clientSocket.EndReceive(ar);//接收数据
                if (count == 0)
                {
                    Close();
                }
                message.ReadMessage(count,OnProcessMessage);//解析数据
                Start();//循环回调
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        //message.ReadMessage的回调函数
        public void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(requestCode, actionCode, data, this);//处理解析好的数据
        }

        //释放当前套接字
        public void Close()
        {
            ConnHelper.CloseConnection(mySqlConn);//释放与数据库的连接
            if (clientSocket != null)
            {
                clientSocket.Close();//关闭套接字
            }          
            if (room != null)
            {
                room.QuitRoom(this);//将当前客户端移除房间
            }
            server.RemoveClient(this);//销毁当前Client对象
        }

        //向套接字发送数据
        public void Send(ActionCode actionCode,string data)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)//如果当前套接字不存在了，则停止发送
                    return;
                byte[] bytes = Message.PackData(actionCode, data);//将数据转换成字节数据
                clientSocket.Send(bytes);//发送数据
            }
            catch (Exception e)
            {
                Console.WriteLine("无法发送消息:" + e);
            }
        }

        public int GetUserId()
        {
            return user.Id;
        }

        //判断当前客户端是否是房主
        public bool IsHouseOwner()
        {
            return room.IsHouseOwner(this);
        }

        //进行伤害计算
        public int TakeDamage(int damage)
        {
            HP -= damage;
            hp = Math.Max(hp, 0);//血量最小值为0，防止血量值小于0
            if (hp <= 0)
            {
                return 0;
            }

            return hp;
        }

        //判断当前玩家角色是否已死亡
        public bool isDie()
        {
            return hp <= 0;
        }

        //同步更新战记到数据库和客户端
        public void UpdateResult(bool isVictory)
        {
            UpdateResultToDB(isVictory);
            UpdateResultToClient();
        }

        //更新战记到数据库
        public void UpdateResultToDB(bool isVictory)
        {
            result.TotalCount++;
            if (isVictory)//如果游戏胜利，则胜利局数加一
            {
                result.WinCount++;
            }
            resultDao.UpdateOrAddResult(mySqlConn, result);//将战记结果更新到数据路
        }

        //更新战记到客户端
        private void UpdateResultToClient()
        {
            Send(ActionCode.UpdateResult,string.Format("{0},{1}",result.TotalCount,result.WinCount));
        }
    }
}
