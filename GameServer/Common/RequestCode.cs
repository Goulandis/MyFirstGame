using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum RequestCode
    {
        None,//默认请求
        User,//与用户信息相关请求
        Room,//与游戏房间相关请求
        Game,//与游戏相关请求
    }
}
