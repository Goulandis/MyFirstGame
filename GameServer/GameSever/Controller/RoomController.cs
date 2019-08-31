using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.SqlServer.Server;
using GameSever.Servers;

namespace GameSever.Controller
{
    //RequestCode.Room映射的Controller类
    class RoomController : BaseController
    {
        
        public RoomController()
        {
            requestCode = RequestCode.Room;//建立映射关系
        }

        //收到创建房间请求时调用
        public string CreateRoom(string data,Client client,Server server)
        {
            Console.WriteLine("创建了一个房间");
            server.CreateRoom(client,data);//当前客户端作为房主创建房间
            //返回反馈码和分配角色类型
            return ((int) ReturnCode.Success).ToString()+","+ ((int)RoleType.Blue).ToString()+","+data;
        }

        //收到刷新房间列表请求时调用
        public string ListRoom(string data, Client client, Server server)
        {
            //将服务器中所有的符合要求的房间信息打包发送给客户端
            StringBuilder sb = new StringBuilder();
            foreach (Room room in server.GetRoomList())
            {
                if (room.IsWaitingJoin())
                {
                    //获取房主信息并打包
                    sb.Append(room.GetHouseOwnerData()+ "," + room.MapIndex +"|");
                }               
            }
            if (sb.Length == 0)
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

       //收到加入房间请求时调用
        public string JoinRoom(string data, Client client, Server server)
        {
            int id = int.Parse(data);//获取要加入房间的房主的客户端ID
            Room room = server.GetRoomById(id);//获取对应ID的房主的房间信息
            //判断当前房间的状态，给与请求客户端相应的反馈
            if (room == null)
            {
                return ((int) ReturnCode.NotFound).ToString();
            }
            else if (room.IsWaitingJoin() == false)
            {
                return ((int) ReturnCode.Fail).ToString();
            }
            else
            {
                room.AddClient(client);//把请求客户端加入到房间中
                string roomData = room.GetRoomData();//获取房间状态信息
                room.BroadcastMessage(client,ActionCode.UpdataRoom,roomData);//向除了请求客户端以外的所有客户端广播，本房间以满员，不再被搜索到
                return ((int) ReturnCode.Success).ToString() + "," + ((int)RoleType.Red).ToString()  + "-" + roomData + "-" + room.MapIndex;
            }
        }

        //当收到退出房间请求时调用
        public string QuitRoom(string data, Client client, Server server)
        {
            bool isHouseOwner = client.IsHouseOwner();//判断是否是房主退出房间
            Room room = client.Room;//获取请求客户端所在的房间信息
            //如果是房主退出房间，则向除房主以外的所有客户端广播，本房间已被解散
            if (isHouseOwner)
            {
                room.BroadcastMessage(client,ActionCode.QuitRoom,((int)ReturnCode.Success).ToString());
                room.Close();
                return ((int)ReturnCode.Success).ToString();
            }
            //如果是房间成员退出房间
            else
            {           
                client.Room.RemoveClient(client);//将请求客户端从本房间中移除
                //向除请求客户端以外的所有客户端广播，本房间处于可加入状态
                room.BroadcastMessage(client, ActionCode.UpdataRoom, room.GetRoomData());
                return ((int) ReturnCode.Success).ToString();
            }
        }
    }
}
