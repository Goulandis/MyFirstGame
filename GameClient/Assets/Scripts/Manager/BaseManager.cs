using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有管理类的基类
public class BaseManager : MonoBehaviour
{
    protected GameFocade facade;
    public BaseManager(GameFocade facade)
    {
        this.facade = facade;
    }

    public virtual void Update()
    {

    }

    public virtual void OnInit()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
