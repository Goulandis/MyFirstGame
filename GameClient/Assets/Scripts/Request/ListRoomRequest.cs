using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;
using UnityEngine;

public class ListRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        List<UserData> userDataList = new List<UserData>();
        if (data != "0")
        {         
            string[] roomArray = data.Split('|');
            foreach (string userData in roomArray)
            {
                //分割房主信息和地图索引，strs[0]-房主ID,strs[1]-房主名字,strs[2]-房主游戏总局数,strs[3]-房主胜利局数，strs[4]-地图索引
                string[] strs = userData.Split(',');
                userDataList.Add(new UserData(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3]),int.Parse(strs[4])));
            }
        }

        roomListPanel.LoadRoomItemSync(userDataList);
    }
}
