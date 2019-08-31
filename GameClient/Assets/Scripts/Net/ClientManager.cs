using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Common;

//专门负责与服务器的连接和数据接收与发送
public class ClientManager : BaseManager {
    //private const string IP = "47.94.254.41";
    private const string IP = "127.0.0.1";
    private const int PORT = 6688;
    private Socket clientSocket;
    private Message msg = new Message();

    public ClientManager(GameFocade facade) : base(facade)
    {

    }

    //初始化并连接服务器
    public override void OnInit()
    {
        base.OnInit();

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }
        catch (Exception e)
        {
            Debug.LogWarning("无法连接到服务器，请检查网络" + e);
        }
    }

    //接收数据
    private void Start()
    {
        clientSocket.BeginReceive(msg.AcceptByte,msg.StartIndex,msg.RemainSize, SocketFlags.None, ReceiveCallback,null);
        
    }

    //clientSocket.BeginReceive的回调函数
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            int count = clientSocket.EndReceive(ar);//接收套接字里的数据
            msg.ReadMessage(count, OnPrecessDataCallback);//解析数据
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    //msg.ReadMessage的回调函数，处理解析好的数据
    private void OnPrecessDataCallback(ActionCode actionCode,string data)
    {
        facade.HandleReponse(actionCode, data);
    }

    //发送数据
    public void SendRequest(RequestCode requestData,ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(requestData, actionCode, data);
        clientSocket.Send(bytes);
    }

    //销毁对象时，释放套接字
    public override void OnDestroy()
    {
        base.OnDestroy();

        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning("无法关闭与服务器的连接" + e);
        }
    }
}
