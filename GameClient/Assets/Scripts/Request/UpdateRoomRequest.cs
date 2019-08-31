using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class UpdateRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.UpdataRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        UserData userData1 = null;
        UserData userData2 = null;
        string[] userStrArray = data.Split('|');
        userData1 = new UserData(userStrArray[0]);
        if(userStrArray.Length>1)
            userData2 = new UserData(userStrArray[1]);
        roomPanel.SetAllPlayerResSync(userData1, userData2);
    }
}
