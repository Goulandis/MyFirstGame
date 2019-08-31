using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using Common;

public class LoginPanel : BasePanle {

    private Button closeButton;
    private InputField usernameIF;
    private InputField passwordIF;
    private Button loginButton;
    private Button registerButton;
    private LoginRequest loginRequest;

    private void Start()
    {
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);
        usernameIF = transform.Find("UserNameLabel/UserNameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(OnLoginClick);
        registerButton = transform.Find("RegisterButton").GetComponent<Button>();
        registerButton.onClick.AddListener(OnRegisterClick);
        loginRequest = GetComponent<LoginRequest>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);
        transform.localPosition = new Vector3(1000, 0, 0);        
        transform.DOLocalMove(Vector3.zero, 0.4f);      
    }

    private void OnLoginClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "\n密码不能为空";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
        }
        else
            loginRequest.SendRequest(usernameIF.text, passwordIF.text);//发送数据到服务器
            
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {       
        if (returnCode == ReturnCode.Success)
        {
            //登陆成功后的操作
            uiMng.PushPanelSync(UIPanelType.RoomList);
        }
        else
        {
            uiMng.ShowMessageSync("用户名密码错误");
        }
    }

    private void OnRegisterClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Register);      
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }

    public override void OnResume()
    {
        base.OnResume();
        OnEnter();
    }

    public override void OnPause()
    {
        HideAnime();
    }

    public override void OnExit()
    {
        HideAnime();
    }

    private void HideAnime()
    {
        transform.DOScale(0, 0.4f);
        transform.DOLocalMoveX(-1000, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }
}
