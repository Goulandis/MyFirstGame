using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class StartPlayRequest : BaseRequest
{
    private bool isStartPlaying = false;

    public override void Awake()
    {
        actionCode = ActionCode.StartPlay;
        base.Awake();
    }

    private void Update()
    {
        if (isStartPlaying == true)
        {
            focade.StartPlaying();
            isStartPlaying = false;
        }
    }

    public override void OnResponse(string data)
    {
        isStartPlaying = true;
    }
}
