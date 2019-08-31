using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuitPlayRequest : BaseRequest {

    private GamePanel gamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.QuitPlay;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }
    public override void SendRequest()
    {
        //因为只需要向服务器发送一个请求，无需发送数据，但TCP套接字不发送空的数据包，故随意发送一个字符串
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        gamePanel.OnGameOverResponse(returnCode);
    }
}
