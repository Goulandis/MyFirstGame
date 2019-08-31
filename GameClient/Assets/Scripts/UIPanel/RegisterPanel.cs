using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using Common;

public class RegisterPanel : BasePanle
{

    private Button closeButton;
    private InputField userNameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;
    private Button registerButton;
    private RegisterRequest registerRequest;

    private void Start()
    {
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        registerButton = transform.Find("RegisterButton").GetComponent<Button>();
        userNameIF = transform.Find("UserNameLabel/UserNameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        rePasswordIF = transform.Find("RePasswordLabel/RePasswordInput").GetComponent<InputField>();
        registerButton.onClick.AddListener(OnRegisterClick);
        closeButton.onClick.AddListener(OnCloseClick);
        registerRequest = GetComponent<RegisterRequest>();
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

    private void OnRegisterClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(userNameIF.text))
        {
            msg += "用户名不能为空";
        }

        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "\n密码不能为空";
        }

        if (!string.IsNullOrEmpty(userNameIF.text) && !string.IsNullOrEmpty(passwordIF.text) && string.IsNullOrEmpty(rePasswordIF.text))
        {
            msg = "请填写复确认密码";
        }

        if (!string.IsNullOrEmpty(userNameIF.text) && !string.IsNullOrEmpty(passwordIF.text) && passwordIF.text != rePasswordIF.text)
        {
            msg = "两次密码填写不一致";
        }

        if (msg != "")
        {
            uiMng.ShowMessage(msg);
        }
        else
        {
            registerRequest.SendRequest(userNameIF.text,passwordIF.text);
        }
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }

    public override void OnExit()
    {
        transform.DOScale(0, 0.4f);
        transform.DOLocalMoveX(-1000, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }

    public void OnRegisterReponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("注册成功");
        }
        else
        {
            uiMng.ShowMessageSync("用户已存在");
        }
    }
}
