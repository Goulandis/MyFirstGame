using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace GameSever.Servers
{
    //房间状态码
    enum RoomState
    {
        WaitingJion,
        WaitingBattle,
        Battling,
        End
    }

    //游戏房间类，一个房间对应一个对象
    class Room
    {
        private List<Client> clientRoom = new List<Client>();//存储房间里的所有客户端
        private RoomState state = RoomState.WaitingJion;//房间的状态标志，默认为等待加入状态
        private Server server;//房间的管理在Server类中，所以获取Server的引用，此处设计的不是很合理，房间管理应该专门创建一个类来管理

        private int mapIndex = 0;//存储当前房间选图的地图索引

        public int MapIndex
        {
            set { mapIndex = value; }
            get { return mapIndex; }
        }

        public Room(Server server)
        {
            this.server = server;
        }

        //向当前房间添加客户端
        public void AddClient(Client client)
        {
            clientRoom.Add(client);//添加客户端
            client.Room = this;//将客户端的Room引用到当前房间
            if (clientRoom.Count >= 2)
            {
                state = RoomState.WaitingBattle;//修改房间状态为等待战斗
            }
        }

        //向当前房间移除客户端
        public void RemoveClient(Client client)
        {
            client.Room = null;//将客户端的Room引用置空
            clientRoom.Remove(client);//移除客户端
            if (clientRoom.Count >= 2)//修改房间状态
            {
                state = RoomState.WaitingBattle;
            }
            else
            {
                state = RoomState.WaitingJion;
            }
        }

        //获取房主信息
        public string GetHouseOwnerData()
        {
            return clientRoom[0].GetUserData();
        }

        //判断当前房间是否是等待加入状态
        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJion;
        }

        //退出房间操作
        public void QuitRoom(Client client)
        {
            if (client == clientRoom[0])//如果客户端是房主则关闭房间
            {
                Close();
            }
            else
            {
                clientRoom.Remove(client);//否则移除当前成员
            }
        }

        //关闭房间
        public void Close()
        {
            foreach (Client client in clientRoom)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
        }

        //获取房主ID
        public int GetId()
        {
            if (clientRoom.Count > 0)
            {
                return clientRoom[0].GetUserId();
            }

            return -1;
        }

        //获取房间信息
        public string GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Client client in clientRoom)
            {
                sb.Append(client.GetUserData() + "|");//组合房间里所有用户的用户信息
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);//删掉房间信息中最后面的一个“|”符号
            }

            return sb.ToString();
        }

        //向除房主外的其他房间成员广播消息
        public void BroadcastMessage(Client excludeClient,ActionCode actionCode,string data)
        {
            foreach (Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    server.SendResponse(client,actionCode,data);
                }
            }
        }

        //向房间里所有成员包括房主，广播消息
        public void BroadcastReturn(Client excludeClient)
        {
            foreach (Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    client.UpdateResult(true);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                }
                else
                {
                    client.UpdateResult(false);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                }
            }            
        }

        //判断传入客户端是否是房主
        public bool IsHouseOwner(Client client)
        {
            return client == clientRoom[0];
        }

        //开启新的线程处理开始倒计时
        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }

        //开始游戏倒计时处理函数
        private void RunTimer()
        {
            Thread.Sleep(1000);
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null,ActionCode.ShowTimer,i.ToString());
                Thread.Sleep(1000);
            }
            BroadcastMessage(null,ActionCode.StartPlay,"r");
        }

        //伤害处理函数
        public void TakeDamage(int damage, Client excludeClient)
        {
            bool isDie = false;
            //标识攻击者与被攻击者
            Client attacker = null;
            Client byAttacker = null;
            foreach (Client client in clientRoom)//判断是否角色死亡
            {
                if (client != excludeClient)
                {
                    byAttacker = client;
                    int hp = client.TakeDamage(damage);
                    if (hp == 0)
                    {
                        isDie = true;
                    }
                }
                else
                {
                    attacker = client;
                }
            }

            if (isDie == false)
            {
                //同步双方玩家的血量值，在前的是己方，在后的是敌方
                string attackerTag = "attacker";
                string byAttackerTag = "byAttacker";
                string attackerData = attacker.HP.ToString() + "," + byAttacker.HP.ToString() + "," + damage.ToString() + "," + attackerTag;
                string byAttackerData = byAttacker.HP.ToString() + "," + attacker.HP.ToString() + "," + damage.ToString() + "," + byAttackerTag;
                //向攻击者与被攻击者发送各自的数据
                attacker.Send(ActionCode.Attack,attackerData );
                byAttacker.Send(ActionCode.Attack, byAttackerData);
                return;
            } 
            foreach (Client client in clientRoom)
            {
                if (client.isDie())//如果有玩家角色死亡,则更新战记并结束游戏
                {
                    client.UpdateResult(false);
                    client.Send(ActionCode.GameOver,((int)ReturnCode.Fail).ToString());
                }
                else
                {
                    client.UpdateResult(true);
                    client.Send(ActionCode.GameOver,((int)ReturnCode.Success).ToString());
                }
            }
            Close();

        }

        //获取房间列表
        public List<Client> GetClientRoom()
        {
            return clientRoom;
        }
    }
}
