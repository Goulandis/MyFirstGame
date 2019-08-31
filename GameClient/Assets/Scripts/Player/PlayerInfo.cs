using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

//挂载到角色游戏物体上，用于存储角色的固定信息
public class PlayerInfo : MonoBehaviour
{
    public RoleType roleType;//记录角色类型
    public bool isLocal = false;//用于区分自身角色和敌方角色
}
