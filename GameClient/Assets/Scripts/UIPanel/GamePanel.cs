using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanle
{
    private Text timer;
    private int time = -1;
    private bool isExitPlay = false;

    private Button successButton;
    private Button failButton;
    private Button exitButton;

    private QuitPlayRequest quitPlayRequest;


    private void Start()
    {
        timer = transform.Find("Timer").GetComponent<Text>();
        timer.gameObject.SetActive(false);
        successButton = transform.Find("SuccessButton").GetComponent<Button>();
        successButton.onClick.AddListener(OnResultClick);
        successButton.gameObject.SetActive(false);
        failButton = transform.Find("FailButton").GetComponent<Button>();
        failButton.onClick.AddListener(OnResultClick);
        failButton.gameObject.SetActive(false);
        exitButton = transform.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(OnExitClick);
        exitButton.gameObject.SetActive(false);
        quitPlayRequest = GetComponent<QuitPlayRequest>();
    }

    private void Update()
    {
        if (time > -1)
        {
            ShowTime(time);
            time = -1;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isExitPlay)
            {
                exitButton.gameObject.SetActive(true);
                exitButton.transform.localScale = Vector3.zero;
                exitButton.transform.DOScale(1, 0.3f).OnComplete(() => Invoke("HideExitButton", 3f));
            }
            //退出游戏
        }
    }

    public override void OnEnter()
    {
        focade.SetIsQuit(false);
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
        successButton.gameObject.SetActive(false);
        failButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    public void ShowTimeSync(int time)
    {
        isExitPlay = true;
        this.time = time;
    }

    public void ShowTime(int time)
    {
        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => timer.gameObject.SetActive(false));
        focade.PlayNomalSound(AudioManager.sound_Alert);
    }

    public void OnGameOverResponse(ReturnCode returnCode)
    {
        Button button = null;
        focade.SetIsQuit(true);
        switch (returnCode)
        {
            case ReturnCode.Success:
                button = successButton;
                break;
            case ReturnCode.Fail:
                button = failButton;
                break;
        }
        if (button != null)
        {
            button.gameObject.SetActive(true);
            button.transform.localScale = Vector3.zero;
            button.transform.DOScale(1, 0.5f);
        }    
    }

    private void OnResultClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        focade.GameOver();
    }

    private void HideExitButton()
    {
        exitButton.gameObject.SetActive(false);
    }

    private void OnExitClick()
    {
        isExitPlay = false;
        exitButton.gameObject.SetActive(false);
        quitPlayRequest.SendRequest();
    }
}
