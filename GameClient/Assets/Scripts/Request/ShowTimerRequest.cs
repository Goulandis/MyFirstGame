using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ShowTimerRequest : BaseRequest
{
    private GamePanel gamePanel;

    public override void Awake()
    {
        actionCode = ActionCode.ShowTimer;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {    
        gamePanel.ShowTimeSync(int.Parse(data));
    }
}
