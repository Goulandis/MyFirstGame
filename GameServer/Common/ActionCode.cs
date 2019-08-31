using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ActionCode
    {
        None,//默认行为
        Login,//登陆行为
        Register,//注册行为
        ListRoom,//刷新房间列表行为
        CreateRoom,//开房请求
        JoinRoom,//加入房间行为
        UpdataRoom,//更新房间信息行为
        QuitRoom,//退出房间行为
        StartGame,//开始游戏行为
        ShowTimer,//游戏倒计时行为
        StartPlay,//游戏可操作行为
        Move,//移动信息同步行为
        Shoot,//射击信息同步行为
        Attack,//攻击行为
        GameOver,//游戏结束行为
        UpdateResult,//更新战记行为
        QuitPlay,//投降行为
    }
}
