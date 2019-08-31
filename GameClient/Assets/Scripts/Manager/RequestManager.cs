using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

//管理请求脚本
public class RequestManager : BaseManager {
    public RequestManager(GameFocade facade) : base(facade)
    {

    }

    //一个行为码映射一个请求脚本
    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    //向字典中添加映射关系
    public void Addrequest(ActionCode actionCode, BaseRequest request)
    {
        if(!requestDict.ContainsKey(actionCode))
            requestDict.Add(actionCode, request);
    }

    //清空字典
    public void RemoveRequest(ActionCode actionCode)
    {
        requestDict.Remove(actionCode);
    }

    //通过行为码寻找所映射的请求脚本
    public void HandleReponse(ActionCode actionCode, string data)
    {
        //使用基类对象统一引用请求脚本
        BaseRequest request = requestDict.TryGet<ActionCode, BaseRequest>(actionCode);
        if (request == null)
        {
            Debug.LogWarning("无法得到ActionCode[" + actionCode + "]对应的类");
            return;
        }
        request.OnResponse(data);//让请求脚本响应客户端的反馈
    }
}
