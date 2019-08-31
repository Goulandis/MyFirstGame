using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

//管理玩家
public class PlayerManager : BaseManager
{
    private UserData localUserData;//存储自身玩家数据
    private UserData enemyUserData;//存储敌方玩家数据
    private Dictionary<RoleType,RoleData> roleDataDic = new Dictionary<RoleType, RoleData>();//存储角色类型和角色数据映射
    private Transform rolePosition;//角色位置
    private RoleType currentRoleType;//当前角色类型
    private GameObject currentRoleGameObject;//自身角色游戏物体
    private GameObject enemyRoleGameObject;//敌方角色游戏物体
    private UGUIFollow localUiFollow;//自身UI跟随脚本引用
    private UGUIFollow enemyUiFollow;//敌方UI跟随脚本引用
    private int localHP;//自身血量值
    private int enemyHP;//敌方血量值
    private GameObject playerSyncRequest;//游戏物，用于挂载玩家数据同步脚本，处理信息同步   
    private MoveRequest moveRequest;//玩家移动信息同步脚本引用
    private ShootRequest shootRequest;//玩家射击信息同步脚本引用
    private PlayerAttack playerAttack;//玩家攻击脚本引用
    private AttackRequest attackRequest;//玩家攻击请求建脚本引用    
    private int mapIndex;//当前房间地图索引

    public PlayerManager(GameFocade facade) : base(facade)
    {
        
    }

    //属性
    public UserData LocalUsreData
    {
        set { localUserData = value; }
        get { return localUserData; }
    }

    public UserData EnemyUsreData
    {
        set { enemyUserData = value; }
        get { return enemyUserData; }
    }

    public UGUIFollow LocalUiFollow
    {
        set { localUiFollow = value; }
        get { return localUiFollow; }
    }

    public UGUIFollow EnemyUiFollow
    {
        set { enemyUiFollow = value; }
        get { return enemyUiFollow; }
    }

    public int LocalHP
    {
        set { localHP = value; }
        get { return localHP; }
    }

    public int EnemyHP
    {
        set { enemyHP = value; }
        get { return enemyHP; }
    }

    //设置自身玩家角色类型
    public void SetCurrentRoleType(RoleType roleType)
    {
        currentRoleType = roleType;
    }

    //设置地图索引
    public void SetMapIndex(int mapIndex)
    {
        this.mapIndex = mapIndex;
    }

    public override void OnInit()
    {

    }

    //将角色的信息加载到角色字典
    public void InitRoleDataDic()
    {
        //加载角色预设，箭矢预设，箭矢爆炸特效预设，初始化位置和地图索引对应的地图
        foreach (RoleType roleType in roleDataDic.Keys)
        {
            if (roleType == RoleType.Blue) return;
            if (roleType == RoleType.Red) return; 
        }
        roleDataDic.Add(RoleType.Blue, new RoleData(RoleType.Blue, "Hunter_Blue", "Arrow_Blue", "Explostion_Blue",mapIndex));
        roleDataDic.Add(RoleType.Red, new RoleData(RoleType.Red, "Hunter_Red", "Arrow_Red", "Explostion_Red",mapIndex));
    }

    //遍历角色字典，实例化角色
    public void SpawnRoles()
    {
        foreach (RoleData roleData in roleDataDic.Values)
        {
            GameObject go = GameObject.Instantiate(roleData.RolePrefab, roleData.SpawnPosition, Quaternion.identity);
            go.tag = "Player";//设置角色标签
            if (roleData.RoleType == currentRoleType)//如果角色类型是自身玩家角色类型，则将此实例化角色标记为自身玩家角色
            {
                currentRoleGameObject = go;
                currentRoleGameObject.GetComponent<PlayerInfo>().isLocal = true;
                //获取UI跟随脚本引用
                localUiFollow = currentRoleGameObject.transform.Find("NamePanel").GetComponent<UGUIFollow>();
                //设置自身角色头顶UI名字
                localUiFollow.SetUserName(facade.GetLocalUserData().UserName);
                localUiFollow.InitHp(localHP);//设置自身角色头顶UI血条
            }

            else
            {
                enemyRoleGameObject = go;
                enemyUiFollow = enemyRoleGameObject.transform.Find("NamePanel").GetComponent<UGUIFollow>();
                enemyUiFollow.SetUserName(facade.GetEnemyUserData().UserName);//设置敌方角色头顶UI名字
                enemyUiFollow.InitHp(enemyHP);//设置敌方角色头顶UI血条
            }
        }
    }

    //获取自身角色游戏物体
    public GameObject GetCurrenRoleGameObject()
    {
        return currentRoleGameObject;
    }

    //获取角色信息
    private RoleData GetRoleData(RoleType roleType)
    {
        RoleData roleData = null;
        roleDataDic.TryGetValue(roleType, out roleData);//根据角色类型从角色字典中获取角色信息
        return roleData;
    }

    //为角色动态添加控制脚本
    public void AddControlScript()
    {
        currentRoleGameObject.AddComponent<PlayerMove>();
        playerAttack = currentRoleGameObject.AddComponent<PlayerAttack>();
        RoleType roleType = currentRoleGameObject.GetComponent<PlayerInfo>().roleType;
        RoleData roleData = GetRoleData(roleType);
        playerAttack.arrow = roleData.ArrowPrefab;
        playerAttack.SetPlayerManager(this);
    }

    //实例化挂载角色信息同步脚本的游戏物体，并未游戏物体添加信息同步脚本
    public void CreateSyncRequest()
    {
        playerSyncRequest = new GameObject("PlayerSyncRequest");
        moveRequest = playerSyncRequest.AddComponent<MoveRequest>();
        moveRequest.SetLocalPlayer(currentRoleGameObject.transform,currentRoleGameObject.GetComponent<PlayerMove>())
            .SetEnemyPlayer(enemyRoleGameObject.transform);
        shootRequest = playerSyncRequest.AddComponent<ShootRequest>();
        shootRequest.playerMng = this;
        attackRequest = playerSyncRequest.AddComponent<AttackRequest>();
    }

    //自身射击行为
    public void LocalShoot(GameObject arrowPrefb, Vector3 pos, Quaternion rot)
    {
        facade.PlayNomalSound(AudioManager.sound_Alert);
        //实力化箭矢
        GameObject.Instantiate(arrowPrefb, pos, rot).GetComponent<Arrow>().isLocal = true;
        //发送射击请求到服务器
        shootRequest.SendRequest(arrowPrefb.GetComponent<Arrow>().roleType,pos,rot.eulerAngles);
    }

    //实例化敌方射击箭矢
    public void EnemyShoot(RoleType roleType, Vector3 pos, Vector3 rot)
    {
        facade.PlayNomalSound(AudioManager.sound_Alert);
        GameObject arrowPrefb = GetRoleData(roleType).ArrowPrefab;
        Transform trans = GameObject.Instantiate(arrowPrefb).GetComponent<Transform>();
        trans.position = pos;
        trans.eulerAngles = rot;
    }

    //异步启动敌方射击动作
    public void EnemySyncAttack()
    {
        moveRequest.EnemyAttack();
    }

    //发送攻击请求
    public void SendAttack(int damage)
    {
        //发送伤害给服务器
        attackRequest.SendRequest(damage);
    }

    //游戏结束销毁对象
    public void GameOver()
    {
        GameObject.Destroy(currentRoleGameObject);
        GameObject.Destroy(playerSyncRequest);
        GameObject.Destroy(enemyRoleGameObject);
        roleDataDic.Clear();
        shootRequest = null;
        attackRequest = null;
    }

    //更新本地用户数据
    public void UpdateResult(int totalCount,int winCount)
    {
        localUserData.TotalCount = totalCount;
        localUserData.WinCount = winCount;
    }
}
