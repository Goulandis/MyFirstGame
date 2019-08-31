using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class LoginRequest : BaseRequest {

    private LoginPanel loginPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }

    public void SendRequest(string username, string password)//打包发送给服务器的数据
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        loginPanel.OnLoginResponse(returnCode);
        if (returnCode == ReturnCode.Success)
        {
            string userName = strs[1];
            int totalCount = int.Parse(strs[2]);
            int winCount = int.Parse(strs[3]);
            UserData userData = new UserData(userName,totalCount,winCount);
            
            focade.SetLocalUserData(userData);
        }
    }
}
