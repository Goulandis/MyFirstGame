using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;
using System.Text;
using System.Linq;

public class Message{

    private byte[] acceptByte = new byte[1024];//接收套接字数据的字节数组
    private int startIndex = 0;//字节数组的存储位置

    public byte[] AcceptByte
    {
        get { return acceptByte; }
    }

    public int StartIndex
    {
        get { return startIndex; }
    }

    //计算数组的剩余空间
    public int RemainSize
    {
        get { return acceptByte.Length - startIndex; }
    }

    //解析数据
    public void ReadMessage(int newDataAmount, Action<ActionCode, string> precessDataCallback)
    {
        startIndex += newDataAmount;//更新位置指针
        while (true)
        {
            if (startIndex <= 4) return;//字节数组的前4个字节存储数据长度，如果位置指针<=4，表示字节数组没有数据
            int count = BitConverter.ToInt32(acceptByte, 0);//获取数据长度
            if ((startIndex - 4) >= count)//如果此条数据是完整的，则进行解析
            {
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(acceptByte, 4);//转换行为码为字节数据
                string s = Encoding.UTF8.GetString(acceptByte, 8, count - 4);//解析反馈数据
                precessDataCallback(actionCode, s);//将解析好的数据回调出去
                Array.Copy(acceptByte, count + 4, acceptByte, 0, startIndex - count - 4);//将剩余的不完整的数据移动到数组的最前端
                startIndex -= count + 4;//更新位置指针

            }
            else
            {
                break;
            }
        }
    }

    //打包请求消息
    public static byte[] PackData(RequestCode requestData, ActionCode actionCode, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestData);//转换请求码为字节数据
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);//转换行为码为字节数据
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);//转换发送数据为字节数据
        int dataAmount = requestCodeBytes.Length + dataBytes.Length + actionCodeBytes.Length;//计算数据长度
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);//将数据长度转换为字节数据
        return dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>()//将字节数据组合起来
                 .Concat(actionCodeBytes).ToArray<byte>()
                 .Concat(dataBytes).ToArray();
    }
}

