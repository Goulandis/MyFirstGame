using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class BaseRequest : MonoBehaviour {

    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    //protected GameFocade focade;
    protected GameFocade _focade;

    protected GameFocade focade
    {
        get
        {
            if (_focade == null)
                _focade = GameFocade.Instance;
            return _focade;
        }
    }

    public virtual void Awake()
    {
        GameFocade.Instance.AddRequest(actionCode, this);
    }


    protected void SendRequest(string data)
    {
        focade.SendRequest(requestCode, actionCode, data);//打包请求、行为和数据
    }

    public virtual void SendRequest()
    {

    }

    public virtual void OnResponse(string data)
    {

    }

    public virtual void OnDestroy()
    {
        if (GameFocade.Instance == null)
            return;
        GameFocade.Instance.RemoveRequest(actionCode);
    }
}
