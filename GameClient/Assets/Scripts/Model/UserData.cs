using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//用户信息，专门负责存储用户信息到本地
public class UserData
{
    public UserData(string userData)
    {
        string[] strs = userData.Split(',');
        Id = int.Parse(strs[0]);
        UserName = strs[1];
        TotalCount = int.Parse(strs[2]);
        WinCount = int.Parse(strs[3]);
    }

    public UserData(string userName, int totalCount, int winCount)
    {
        UserName = userName;
        TotalCount = totalCount;
        WinCount = winCount;
    }

    public UserData(int id ,string userName, int totalCount, int winCount,int mapIndex)
    {
        Id = id;
        UserName = userName;
        TotalCount = totalCount;
        WinCount = winCount;
        MapIndex = mapIndex;
    }

    public int Id { get; set; }
    public string UserName { get; set; }
    public int TotalCount { get; set; }
    public int WinCount { get; set; }
    public int MapIndex { get; set; }
}
