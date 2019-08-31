using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using Common;

public class StartGameRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode) int.Parse(strs[0]);
        if (returnCode == ReturnCode.Success)
        {
            int localHP = int.Parse(strs[1]);
            int enemyHP = int.Parse(strs[2]);
            roomPanel.OnStartResponse(returnCode, localHP, enemyHP);
        }
        else
        {
            roomPanel.OnStartResponse(returnCode);
        }
    }
}
