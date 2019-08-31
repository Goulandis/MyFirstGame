using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanle
{
    private Text localPlayerUserName;
    private Text localPlayerWinRate;

    private Text enemyPlayerUserName;
    private Text enemyPlayerWinRate;

    private Transform bluePanel;
    private Transform redPanel;
    private Transform startGame;
    private Transform exitRoom;

    private Button startButton;
    private Button exitButton;

    private Animator startButtonAnime;
    private Animator exitButtonAnime;

    private UserData userData = null;
    private UserData localUserData = null;
    private UserData enemyUserData = null;

    private QuitRoomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;
    private bool isPopPanel = false;

    // Use this for initialization
    private void Start()
    {
        localPlayerUserName = transform.Find("BluePanel/UserName").GetComponent<Text>();
        localPlayerWinRate = transform.Find("BluePanel/WinRate").GetComponent<Text>();

        enemyPlayerUserName = transform.Find("RedPanel/UserName").GetComponent<Text>();
        enemyPlayerWinRate = transform.Find("RedPanel/WinRate").GetComponent<Text>();

        bluePanel = transform.Find("BluePanel");
        redPanel = transform.Find("RedPanel");
        startGame = transform.Find("StartGame");
        exitRoom = transform.Find("ExitRoom");

        startButtonAnime = transform.Find("StartGame").GetComponent<Animator>();
        exitButtonAnime = transform.Find("ExitRoom").GetComponent<Animator>();

        startButton = transform.Find("StartGame").GetComponent<Button>();
        exitButton = transform.Find("ExitRoom").GetComponent<Button>();
        startButton.onClick.AddListener(OnStartClick);
        exitButton.onClick.AddListener(OnExitClick);

        quitRoomRequest = GetComponent<QuitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();

        EnterAnime();
    }

    private void Update()
    {
        if (userData != null)
        {
            SetLocalPlayerRes(userData.UserName,userData.TotalCount,userData.WinCount);
            ClearEnemyPlayerRes();
            userData = null;
       }

        if (localUserData != null)
        {
            SetLocalPlayerRes(localUserData.UserName , localUserData.TotalCount, localUserData.WinCount);
            if (enemyUserData != null)
            {
                focade.SetEnemyUserData(enemyUserData);
                SetEnemyPlayerRes(enemyUserData.UserName, enemyUserData.TotalCount, enemyUserData.WinCount);
            }
            else
            {
                ClearEnemyPlayerRes();
            }
            localUserData = null;
            enemyUserData = null;
        }

        if (isPopPanel)
        {
            uiMng.PopPanel();
            isPopPanel = false;
        }
    }

    public void SetAllPlayerResSync(UserData localUserData, UserData enemyUserData)
    {
        this.localUserData = localUserData;
        this.enemyUserData = enemyUserData;
    }

    public void SetLocalPlayerResSync()
    {
        userData = focade.GetLocalUserData();
    }

    public void SetLocalPlayerRes(string userName, int totalCount, int winCount)
    {
        localPlayerUserName.text = userName;
        if (totalCount == 0)
        {
            localPlayerWinRate.text = "胜率\n0";
        }
        else
        {
            localPlayerWinRate.text = "胜率\n" + ((float)winCount / totalCount).ToString("f2");
        }
    }

    public void SetEnemyPlayerRes(string userName, int totalCount, int winCount)
    {
        enemyPlayerUserName.text = userName;
        if (totalCount == 0)
        {
            enemyPlayerWinRate.text = "胜率\n0";
        }
        else
        {
            enemyPlayerWinRate.text = "胜率\n" + ((float)winCount / totalCount).ToString("f2");
        }       
    }

    public void ClearEnemyPlayerRes()
    {
        enemyPlayerUserName.text = "等待玩家加入......";
        enemyPlayerWinRate.text = "";
    }

    private void OnStartClick()
    {
        PlayClickSound();
        startGameRequest.SendRequest();
    }

    public void OnStartResponse(ReturnCode returnCode,int localHP,int enemyHP)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("房主未开启游戏");
        }
        else
        {
            uiMng.PushPanelSync(UIPanelType.Game);
            focade.EnterPlayingSync(localHP,enemyHP);//实例化角色并Camera跟随角色
        }
    }

    public void OnStartResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("房主未开启游戏");
        }
        else
        {
            uiMng.PushPanelSync(UIPanelType.Game);
        }
    }

    private void OnExitClick()
    {
        PlayClickSound();
        exitButtonAnime.enabled = false;
        quitRoomRequest.SendRequest();
    }

    public void OnExitResponse()
    {
        isPopPanel = true;
    }

    public override void OnEnter()
    {
        if (bluePanel != null)
        {
            EnterAnime();
        }
    }

    public override void OnPause()
    {
        ExitAnime();
    }

    public override void OnResume()
    {
        EnterAnime();
    }

    public override void OnExit()
    {
        
        ExitAnime();
    }

    private void EnterAnime()
    {
        gameObject.SetActive(true);
        startButtonAnime.enabled = false;
        exitButtonAnime.enabled = false;

        bluePanel.localPosition = new Vector3(-1000,63,0);
        bluePanel.DOLocalMoveX(-127, 0.4f);

        redPanel.localPosition = new Vector3(1000,63,0);
        redPanel.DOLocalMoveX(120, 0.4f);

        startGame.localScale = Vector3.zero;
        startGame.DOScale(1, 0.4f).OnComplete(()=>startButtonAnime.enabled = true);

        exitRoom.localScale = Vector3.zero;
        exitRoom.DOScale(1, 0.4f).OnComplete(()=>exitButtonAnime.enabled = true);
    }

    private void ExitAnime()
    {
        startButtonAnime.enabled = false;
        exitButtonAnime.enabled = false;
        bluePanel.DOLocalMoveX(-2000, 0.4f);
        redPanel.DOLocalMoveX(-2000, 0.4f);
        startGame.DOScale(0, 0.4f);
        exitRoom.DOScale(0, 0.4f);
        gameObject.SetActive(false);
    }

    //private void GetUserName(string localUserName, string enemyUserName)
    //{
    //    focade.GetUserName
    //}
}
