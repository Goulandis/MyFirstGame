using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameSever.Controller;
using Common;
namespace GameSever.Servers
{
    //负责接收客户端和管理房间
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        List<Client> clientList = new List<Client>();
        private ControllerManager controllerManager;
        private List<Room> roomList = new List<Room>();

        public Server() { }
        public Server(string ip, int port)
        {
            controllerManager = new ControllerManager(this);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
        }

        public void Start() {
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, serverSocket);//异步接收连接请求

        }

        void AcceptCallback(IAsyncResult ar)
        {
            Console.WriteLine("一个客户端已连接");
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client); 
            Start();
        }

        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }

        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            client.Send(actionCode, data);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }

        public void CreateRoom(Client client,string data)
        {
            Room room = new Room(this);
            room.AddClient(client);
            room.MapIndex = int.Parse(data);
            Console.WriteLine("地图索引："+room.MapIndex);
            roomList.Add(room);
        }

        public void RemoveRoom(Room room)
        {
            if (roomList != null && room != null)
            {
                roomList.Remove(room);
            }
        }

        public List<Room> GetRoomList()
        {
            return roomList;
        }

        public Room GetRoomById(int id)
        {
            foreach (Room room in roomList)
            {
                if (room.GetId() == id)
                    return room;
            }

            return null;
        }
    }
}
