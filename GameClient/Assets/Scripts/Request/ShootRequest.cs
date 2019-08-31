using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using UnityEngine;
using Common;

public class ShootRequest : BaseRequest
{
    public PlayerManager playerMng;
    private bool isShoot = false;
    private RoleType roleType;
    private Vector3 pos;
    private Vector3 rot;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Shoot;
        base.Awake();
    }

    void Start () {
		
	}

	void Update () {
	    if (isShoot)
	    {
            playerMng.EnemySyncAttack();
            Invoke("EnemyShoot", 0.5f);           
	        isShoot = false;
	    }
	}

    public void SendRequest(RoleType roleType,Vector3 pos ,Vector3 rot)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)roleType, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z);
        base.SendRequest(data);
    }

    //接收到服务器反馈时被调用
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        RoleType roleType = (RoleType) int.Parse(strs[0]);
        Vector3 pos = UnityTools.Parse(strs[1]);
        Vector3 rot = UnityTools.Parse(strs[2]);
        isShoot = true;
        this.roleType = roleType;
        this.pos = pos;
        this.rot = rot;
    }

    private void EnemyShoot()
    {
        playerMng.EnemyShoot(roleType, pos, rot);
    }
}
