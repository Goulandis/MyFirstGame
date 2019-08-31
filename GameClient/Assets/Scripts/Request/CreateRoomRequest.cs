using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class CreateRoomRequest : BaseRequest
{
   private RoomPanel roomPanel;

   public override void Awake()
   {
       requestCode = RequestCode.Room;
       actionCode = ActionCode.CreateRoom;
       base.Awake();
   }

    //获取当前入栈的UIPanel
    public void SetPanel(BasePanle panel)
    {
        roomPanel = panel as RoomPanel;
    }

    //向服务器发送请求
    public new void SendRequest(string data)
    {
        //因为只需要向服务器发送一个请求，无需发送数据，但TCP套接字不发送空的数据包，故随意发送一个字符串
        base.SendRequest(data);
    }

    //接收到服务器反馈时被调用
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');//strs[0]-反馈码，strs[1]-角色类型，strs[2]-地图索引
        ReturnCode returnCode = (ReturnCode) int.Parse(strs[0]);
        RoleType roleType = (RoleType) int.Parse(strs[1]);
        focade.SetCurrentRoleTypeAndMapIndex(roleType,int.Parse(strs[2]));//将服务器分配的角色类型设置到本地
        //UserData.MapIndex = int.Parse(strs[2]);
        if (returnCode == ReturnCode.Success)
        {
            //异步设置玩家信息，因为UI显示必须运行在主线程中，故使用异步设置
            roomPanel.SetLocalPlayerResSync();
        }
    }
}
