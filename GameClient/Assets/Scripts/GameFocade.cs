using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GameFocade : MonoBehaviour {//GameFocade类是所有类的中介，所有类通过focade联系起来

    private static GameFocade _instance;

    private UIManager uiMng;//整体UI管理
    private AudioManager audioMng;//声音管理
    private PlayerManager playerMng;//玩家管理
    private CameraManager cameraMng;//相机管理
    private RequestManager requestMng;//服务器与客户端的请求应答管理
    private ClientManager clientMng;//服务器与客户端连接管理
    private bool isEnterPlaying = false;
    private bool isInitRoleDataDic = false;
    private bool isQuit = true;

    public static GameFocade Instance
    {
        get {
            if (_instance == null)
            {
                GameObject obj = GameObject.Find("GameFocade");
                if (obj == null)
                    return null;
                _instance =obj.GetComponent<GameFocade>();
            }
            return _instance;
        }
    }

    void Start () {
        InitManager();//初始化管理脚本
	}

    void Update()
    {
        UpdateManager();
        if (isEnterPlaying)
        {
            EnterPlaying();
            isEnterPlaying = false;
        }
        if (isInitRoleDataDic)
        {
            playerMng.InitRoleDataDic();
            isInitRoleDataDic = false;
        }

        if (isQuit)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiMng.PushPanel(UIPanelType.Quit);
            }
        }

    }

    public void SetIsQuit(bool isQuit)
    {
        this.isQuit = isQuit;
    }

    private void OnDestroy()
    {
        DestroyManager();
    }

    public void InitManager()
    {
        uiMng = new UIManager(this);//实例化整体UI管理对象
        audioMng = new AudioManager(this);
        playerMng = new PlayerManager(this);
        cameraMng = new CameraManager(this);
        requestMng = new RequestManager(this);
        clientMng = new ClientManager(this);

        uiMng.OnInit();
        audioMng.OnInit();
        playerMng.OnInit();
        cameraMng.OnInit();
        requestMng.OnInit();
        clientMng.OnInit();
    }

    public void DestroyManager()
    {
        uiMng.OnDestroy();
        audioMng.OnDestroy();
        playerMng.OnDestroy();
        cameraMng.OnDestroy();
        requestMng.OnDestroy();
        clientMng.OnDestroy();
    }

    private void UpdateManager()
    {
        uiMng.Update();
        audioMng.Update();
        playerMng.Update();
        cameraMng.Update();
        requestMng.Update();
        clientMng.Update();
    }

    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        requestMng.Addrequest(actionCode, request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestMng.RemoveRequest(actionCode);
    }

    public void HandleReponse(ActionCode actionCode, string data)
    {
        requestMng.HandleReponse(actionCode, data);
    }

    public void ShowMessage(string msg)
    {
        uiMng.ShowMessage(msg);
    }

    public void SendRequest(RequestCode requestData, ActionCode actionCode, string data)
    {
        clientMng.SendRequest(requestData, actionCode,data);
    }

    public void PlayBgmSound(string soundName)
    {
        audioMng.PlayBgmSound(soundName);
    }

    public void PlayNomalSound(string soundName)
    {
        audioMng.PlayNomalSound(soundName);
    }

    public void SetLocalUserData(UserData userData)
    {
        playerMng.LocalUsreData = userData;
    }

    public void SetEnemyUserData(UserData userData)
    {
        playerMng.EnemyUsreData = userData;
    }

    public UserData GetLocalUserData()
    {
        return playerMng.LocalUsreData;
    }

    public UserData GetEnemyUserData()
    {
        return playerMng.EnemyUsreData;
    }

    public void SetHPAndDamage(int localPlayerHP,int enemyPlayerHP,int damage,string attackTag)
    {
        if (attackTag == "attacker")
        {
            playerMng.LocalUiFollow.SetHPAndDamage(localPlayerHP);
            playerMng.EnemyUiFollow.SetHPAndDamage(enemyPlayerHP, damage);
        }
        if (attackTag == "byAttacker")
        {
            playerMng.LocalUiFollow.SetHPAndDamage(localPlayerHP,damage);
            playerMng.EnemyUiFollow.SetHPAndDamage(enemyPlayerHP);
        }
       
    }

    public void SetCurrentRoleTypeAndMapIndex(RoleType roleType,int mapIndex)
    {
        playerMng.SetCurrentRoleType(roleType);
        playerMng.SetMapIndex(mapIndex);
        isInitRoleDataDic = true;
    }

    public GameObject GetCurrenRoleGameObject()
    {
        return playerMng.GetCurrenRoleGameObject();
    }

    //将PlayerManager中的SpawnRoles和CameraManager的FollowRole方法联系起来
    public void EnterPlaying()
    {
        playerMng.SpawnRoles();
        cameraMng.FollowRole();
    }

    public void EnterPlayingSync(int localHP,int enemyHP)
    {
        playerMng.LocalHP = localHP;
        playerMng.EnemyHP = enemyHP;
        isEnterPlaying = true;
    }

    public void StartPlaying()
    {
        playerMng.AddControlScript();
        playerMng.CreateSyncRequest();
    }

    public void SendAttack(int damage)
    {
        playerMng.SendAttack(damage);
    }

    public void GameOver()
    {
        cameraMng.WalkthroughScene();
        playerMng.GameOver();
    }

    public void UpdateResult(int totalCount, int winCount)
    {
        playerMng.UpdateResult(totalCount,winCount);
    }
}
