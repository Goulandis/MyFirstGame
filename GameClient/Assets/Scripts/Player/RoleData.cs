using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Assets.Scripts.Map;

//保存角色的各种状态信息
public class RoleData
{
    private const string ROLE_PATH = "Prefbs/";
    private const string ROLEPOS_PATH = "RolePosition/";
    public RoleType RoleType { get; private set; }
    public GameObject RolePrefab { get; private set; }
    public GameObject ArrowPrefab { get; private set; }
    public Vector3 SpawnPosition { get; private set; }
    public GameObject ExplostionEffect { get; private set; }
    private List<Transform> mapList = new List<Transform>();
    private MapInfo mapInfo;

    public RoleData(RoleType roleType, string rolePath, string arrowPath, string effectPath,int mapIndex)
    {
        RoleType = roleType;
        RolePrefab = Resources.Load(ROLE_PATH + rolePath) as GameObject;
        ArrowPrefab = Resources.Load(ROLE_PATH + arrowPath) as GameObject;
        ExplostionEffect = Resources.Load(ROLE_PATH + effectPath) as GameObject;
        ArrowPrefab.GetComponent<Arrow>().explostionEffect = ExplostionEffect;
        mapInfo = new MapInfo(mapIndex, roleType);
        SpawnPosition = mapInfo.CurrentRolePos.position;
    }

}
