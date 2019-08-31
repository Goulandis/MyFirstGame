using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using UnityEngine;
using Common;

public class MoveRequest : BaseRequest
{
    private Transform playerTrans;
    private PlayerMove playerMove;
    private Transform enemyTrans;
    private Animator enemyAnime;
    private int syncRate = 40;

    private bool isSyncEnemyPlayer = false;
    private Vector3 pos;
    private Vector3 rot;
    private float forward;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }

    private void Start()
    {
        InvokeRepeating("SyncLocalPlayer", 1f, 1f / syncRate);
    }

    private void FixedUpdate()
    {
        if (isSyncEnemyPlayer)
        {
            SyncEnemyPlayer();
            isSyncEnemyPlayer = false;
        }
    }

    private void SyncLocalPlayer()
    {
        SendRequest(playerTrans.position.x,playerTrans.position.y,playerTrans.position.z,
            playerTrans.eulerAngles.x,playerTrans.eulerAngles.y,playerTrans.eulerAngles.z, playerMove.forward);
    }

    public MoveRequest SetLocalPlayer(Transform playerTrans,PlayerMove playerMove)
    {
        this.playerTrans = playerTrans;
        this.playerMove = playerMove;
        return this;
    }

    public MoveRequest SetEnemyPlayer(Transform enemyTrans)
    {
        this.enemyTrans = enemyTrans;
        this.enemyAnime = enemyTrans.GetComponent<Animator>();
        return this;
    }

    private void SendRequest(float x, float y, float z, float rotationX, float rotationY, float rotationZ,float forword)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ, forword);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        pos = UnityTools.Parse(strs[0]);
        rot = UnityTools.Parse(strs[1]);
        forward = float.Parse(strs[2]);
        isSyncEnemyPlayer = true;
    }

    public void SyncEnemyPlayer()
    {
        enemyTrans.position = pos;
        enemyTrans.eulerAngles = rot;
        enemyAnime.SetFloat("Forward",forward);
    }

    public void EnemyAttack()
    {
        enemyAnime.SetTrigger("Attack");
    }
}
