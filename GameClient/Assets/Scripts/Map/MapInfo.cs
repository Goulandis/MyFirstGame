using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common;

namespace Assets.Scripts.Map
{
    //地图信息，专门负责处理地图的加载
    class MapInfo
    {
        public string MapName { get; set; }
        public Sprite MapSprite { get; set; }
        public Transform CurrentRolePos { get; set; }
        private  GameObject rolePosition;

        //通过地图索引和角色类型加载各种地图信息
        public MapInfo(int mapIndex,RoleType roleType)
        {
            switch (mapIndex)
            {
                case 0:
                    MapName = "大洋小镇";
                    MapSprite = Resources.Load<Sprite>("Map/Map_Havana");                    
                    if (roleType != RoleType.None)
                    {
                        rolePosition = GameObject.Find("RolePosition/Havana_RolePosition");
                        if (roleType == RoleType.Blue)
                        {
                            CurrentRolePos = rolePosition.transform.Find("PositionBlue").GetComponent<Transform>();
                        }
                        if(roleType == RoleType.Red)
                        {
                            CurrentRolePos = rolePosition.transform.Find("PositionRed").GetComponent<Transform>();
                        }
                    }                   
                    
                    break;
                case 1:
                    MapName = "海景别墅";
                    MapSprite = Resources.Load<Sprite>("Map/Map_Mansion");
                    if (roleType != RoleType.None)
                    {
                        rolePosition = GameObject.Find("RolePosition/Mansion_RolePosition");
                        if (roleType == RoleType.Blue)
                        {
                            CurrentRolePos = rolePosition.transform.Find("PositionBlue").GetComponent<Transform>();
                        }
                        if(roleType == RoleType.Red)
                        {
                            CurrentRolePos = rolePosition.transform.Find("PositionRed").GetComponent<Transform>();
                        }
                    }                   
                    
                    break;
                case 2:
                    MapName = "海盗营地";
                    MapSprite = Resources.Load<Sprite>("Map/Map_Shanty");
                    if (roleType != RoleType.None)
                    {
                        rolePosition = GameObject.Find("RolePosition/Shanty_RolePosition");
                        if (roleType == RoleType.Blue)
                        {
                            CurrentRolePos = rolePosition.transform.Find("PositionBlue").GetComponent<Transform>();
                        }
                        if(roleType == RoleType.Red)
                        {
                            CurrentRolePos = rolePosition.transform.Find("PositionRed").GetComponent<Transform>();
                        }
                    }                 
                    
                    break;
                case 3:
                    MapName = "骷髅岛";
                    MapSprite = Resources.Load<Sprite>("Map/Map_Shipwreck");
                    if (roleType != RoleType.None)
                    {
                        rolePosition = GameObject.Find("RolePosition/Shipwreck_RolePosition");
                        if (roleType == RoleType.Blue)
                        {
                            CurrentRolePos = rolePosition.transform.Find("PositionBlue").GetComponent<Transform>();
                        }
                        if(roleType == RoleType.Red)
                        {
                            CurrentRolePos = rolePosition.transform.Find("PositionRed").GetComponent<Transform>();
                        }
                    }
                    break;
            }
        }
    }
}
