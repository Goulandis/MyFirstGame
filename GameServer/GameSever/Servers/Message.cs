using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameSever.Servers
{
    //专门负责套接字的数据解析
    class Message
    {
        private byte[] acceptByte = new byte[1024];//存储套接字的字节数据
        private int startIndex = 0;//指向数组存储到的位置
        
        public byte[] AcceptByte
        {
            get { return acceptByte; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        //计算存储数组的剩余容量
        public int RemainSize
        {
            get { return acceptByte.Length - startIndex; }
        }

        //解析数据
        public void ReadMessage(int newDataAmount,Action<RequestCode,ActionCode,string> proccessDaraCallback)
        {
            startIndex += newDataAmount;//更新位置指针
            while (true)
            {
                if (startIndex <= 4) return;//前面四位存储一条完整数据的长度，当位置指针<=4时表示没有数据
                int count = BitConverter.ToInt32(acceptByte, 0);//从数组的0位置开始转换4个字节的字节数据为Int数据
                if ((startIndex + 4) >= count)//判断数组里的数据是否时完整的一条数据
                {
                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(acceptByte, 4);//解析请求码
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(acceptByte, 8);//解析行为码
                    string s = Encoding.UTF8.GetString(acceptByte, 12, count - 8);//解析用户数据
                    proccessDaraCallback(requestCode, actionCode, s);//将解析好的数据回调出去
                    Array.Copy(acceptByte, count + 4, acceptByte, 0, startIndex - count - 4);//将剩余的不完整的数据移动到数组的最前端
                    startIndex -= count + 4;//更新位置指针
                }
                else
                {
                    break;
                }
            }
        }

        //打包服务器的反馈数据
        public static byte[] PackData(ActionCode actionData, string data)
        {
            byte[] actionCodeBytes = BitConverter.GetBytes((int)actionData);//转换行为码为字节数据
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);//转换反馈数据为字节数据
            int dataAmount = actionCodeBytes.Length + dataBytes.Length;//计算数据长度
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);//将数据长度转换为字节数据
            byte[] newBytes = dataAmountBytes.Concat(actionCodeBytes).ToArray<byte>();//将字节数据组合起来
            return newBytes.Concat(dataBytes).ToArray<byte>();
        }
    }
}
