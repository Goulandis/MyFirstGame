using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameSever.Servers;

namespace GameSever.Controller
{
    //RequestCode.Game请求映射Controller类
    class GameController : BaseController
    {
        public void RoomController()
        {
            requestCode = RequestCode.Game;//GameController类与RequestCode.Game建立映射关系
        }

        //对应ActionCode.StartGame行为的处理函数
        public string StartGame(string data, Client client, Server server)
        {
            Client HouseOwner = null;
            Client HouseMember = null;
            Room room = client.Room;//获取当前客户端所在的房间
            //遍历房间成员获取房主与成员客户端
            foreach (Client temp in room.GetClientRoom())
            {
                if (temp.IsHouseOwner())
                {
                    HouseOwner = temp;
                }
                else
                {
                    HouseMember = temp;
                } 
            }
            //只有房主才可以开启游戏
            if (client.IsHouseOwner())
            {
                Console.WriteLine("房主开启了游戏");
                //返回双方玩家的血量，在前的是己方，在后的是敌方
                string dataTemp = ((int)ReturnCode.Success).ToString() + "," + HouseMember.HP.ToString() + "," + HouseOwner.HP.ToString();
                room.BroadcastMessage(client,ActionCode.StartGame,dataTemp);//通知成员客户端开始游戏
                room.StartTimer();//开启游戏后自动同步双方客户端的倒计时，倒计时由服务器处理
                return ((int) ReturnCode.Success).ToString() + "," + HouseOwner.HP.ToString() + "," + HouseMember.HP.ToString();
            }
            else
            {
                return ((int) ReturnCode.Fail).ToString();
            }
        }

        //对应ActionCode.Move行为的处理函数，只做数据转发不做数据处理
        public string Move(string data, Client client, Server server)
        {
            Room room = client.Room;
            if(room != null)
                room.BroadcastMessage(client,ActionCode.Move,data);
            return null;
        }

        //对应ActionCode.Shoot行为的处理函数，只做数据转发不做数据处理
        public string Shoot(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadcastMessage(client, ActionCode.Shoot, data);
            return null;
        }

        //对应ActionCode.Attack的处理函数，进行伤害计算
        public void Attack(string data,Client client,Server server)
        {
            int damage = int.Parse(data);
            Room room = client.Room;
            if(room == null)
                return ;
            room.TakeDamage(damage,client);//在服务器段处理伤害计算
            return;
        }

        //对应ActionCode.QuitPlay的处理函数
        public void QuitPlay(string data, Client client, Server server)
        {
            Room room = client.Room;//获取请求客户端所在的房间信息
            room.BroadcastReturn(client);//向房间里的所有客户端广播战斗结果
            room.Close();//从服务器的房间列表中销毁当前房间
        }
    }
}
