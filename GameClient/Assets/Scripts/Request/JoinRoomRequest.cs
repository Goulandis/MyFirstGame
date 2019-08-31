using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

//专门用于处理加入房间的请求和加入房间的反馈的处理
public class JoinRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        //设置JoinRoomRequest的请求码为Room，对应服务器的处理类RommCotroller
        requestCode = RequestCode.Room;
        //设置JoinRoomRequest的行为码为JoinRoom,对应RoomCotroller处理类的JoinRoom处理方法
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    //发送加入房间请求，将要加入房间的id发动给服务器
    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }

    //处理加入房间反馈
    public override void OnResponse(string data)
    {
        //将反馈码、分配的角色类型,房间用户信息和地图信息分割，strs1[0]存储反馈码和分配角色类型，strs[1]存储房间用户信息，strs[2]存储地图信息
        string[] strs1 = data.Split('-');
        //将反馈码与分配角色类型分割，strs2[0]存储反馈码，strs2[1]存储分配角色类型
        string[] strs2 = strs1[0].Split(',');
        ReturnCode returnCode = (ReturnCode) int.Parse(strs2[0]);
        UserData localUserData = null;
        UserData enemyUserData = null;
        if (returnCode == ReturnCode.Success)
        {
            //将房间中各个成员的信息分割，并存储到UserData对象中
            string[] userStrArray = strs1[1].Split('|');
            enemyUserData = new UserData(userStrArray[0]);
            localUserData = new UserData(userStrArray[1]);
            RoleType roleType = (RoleType) int.Parse(strs2[1]);
            //将服务器分配的角色类型设置到本地
            focade.SetCurrentRoleTypeAndMapIndex(roleType,int.Parse(strs1[2]));
        }
        //对加入房间反馈做加入房间处理
        roomListPanel.OnJoinResponse(returnCode, localUserData, enemyUserData);
    }
}
