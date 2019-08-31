using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class AttackRequest : BaseRequest
{

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Attack;
        base.Awake();
    }

    public void SendRequest(int damage)
    {
        base.SendRequest(damage.ToString());
    }

    public override void OnResponse(string data)
    {
        //strs[0]-自身玩家血量值，strs[1]-敌方玩家血量值，strs[2]-伤害值
        string[] strs = data.Split(',');
        int localPlayerHP = int.Parse(strs[0]);
        int enemyPlayerHP = int.Parse(strs[1]);
        int damage = int.Parse(strs[2]);
        string attackTag = strs[3];
        focade.SetHPAndDamage(localPlayerHP, enemyPlayerHP, damage, attackTag);     
    }
}
