using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//管理UI，使用栈结构管理UI界面
public class UIManager : BaseManager
{
    private Transform canvasTransform;//UI界面挂载的父物体引用
    private Transform CanvasTransform//此引用使用单例模式
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    private Dictionary<UIPanelType, string> panelPathDict;//UI路径字典，存储UI界面的加载路径
    private Dictionary<UIPanelType, BasePanle> panelDict;//UI字典，存储UI界面
    private Stack<BasePanle> panelStack;//界面栈
    private MessagePanel msgPanel;//提示消息面板
    private UIPanelType panelTypePush = UIPanelType.None;//UI界面类型，默认为None-无UI界面

    public override void OnInit()//初始化UIManager
    {
        base.OnInit();
        PushPanel(UIPanelType.Message);//将MessagePanel界面入栈
        PushPanel(UIPanelType.Start);//将StartPanel界面入栈
    }

    public override void Update()
    {
        if (panelTypePush != UIPanelType.None)//使用Updata函数实现异步加载UI界面
        {
            PushPanel(panelTypePush);
            panelTypePush = UIPanelType.None;
        }
    }

    public UIManager(GameFocade facade) : base(facade)//传递GameFacode给BaseManager，使BaseManager持有GameFacode的引用
    {
        ParseUIPanelTypeJson();
    }

    //界面入栈
    public BasePanle PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)//判断界面栈对象是否存在，如果不存在则实例化一个对象
            panelStack = new Stack<BasePanle>();
        if (panelStack.Count > 0)//界面栈判空
        {
            BasePanle topPanel = panelStack.Peek();//获取栈顶的值
            topPanel.OnPause();         
        }

        BasePanle panel = GetPanel(panelType);//获取panelType相关联的界面对象
        panel.OnEnter();
        panelStack.Push(panel);//将界面对象压入界面栈
        return panel;
    }

    //异步加载UI界面
    public void PushPanelSync(UIPanelType panelType)
    {
        panelTypePush = panelType;
    }

    //界面出栈
    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanle>();
        }

        if (panelStack.Count <= 0)
            return;

        BasePanle topPanel = panelStack.Pop();        
        topPanel.OnExit();

        if (panelStack.Count <= 0)
            return;

        BasePanle topPanel2 = panelStack.Peek();
        topPanel2.OnResume();

    }

    //通过界面类型获取UI界面
    private BasePanle GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)//界面字典判空，如果为空则实例化对象
        {
            panelDict = new Dictionary<UIPanelType, BasePanle>();
        }

        BasePanle panel = panelDict.TryGet(panelType);//获取panelType存放的键值相关联的对象
        //Debug.Log(panel);
        if (panel == null)//将使用过的UI界面对象转存到Dictionary<UIPanelType, BasePanle>字典中，省去每次加载界面都通过path路径从UIPanelType.json里面读取
        {
            string path = panelPathDict.TryGet(panelType);//从Dictionary<UIPanelType, string>字典中加载panelType保存的键值相关联的路径
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;//通过路径寻找到游戏物体
            instPanel.transform.SetParent(CanvasTransform, false);//将CanvasTransform游戏物体设置成instPanel所代表的游戏物体的父节点
            instPanel.GetComponent<BasePanle>().UIMng = this;//使BasePanel持有本对象--UIManager对象的引用
            instPanel.GetComponent<BasePanle>().Focade = facade;
            panelDict.Add(panelType, instPanel.GetComponent<BasePanle>());//向Dictionary<UIPanelType, BasePanle>字典加入数据
            return instPanel.GetComponent<BasePanle>();
        }
        else
        {
            return panel;
        }
    }

    [Serializable]//可序列化特性，以存储UI界面信息
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }

    //将PanelTypeJson文本中的UI界面加载路径实例化出来，并转存到界面路径字典中
    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();//实例化字典对象
        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");
        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

        foreach (UIPanelInfo info in jsonObject.infoList)
        {
            if (!panelPathDict.ContainsKey(info.panelType))
            {
                panelPathDict.Add(info.panelType, info.path);
            }
        }
    }

    //获取MessagePanel界面的引用
    public void InjectMsgPanel(MessagePanel msgPanel)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，MessagePanel为空");
            return;
        }
        this.msgPanel = msgPanel;
    }

    //显示提示信息
    public void ShowMessage(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，MsgPanel为空");
        }
        msgPanel.ShowMessage(msg);
    }

    //异步显示提示信息
    public void ShowMessageSync(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，MsgPanel为空");
        }
        msgPanel.ShowMessageSync(msg);
    }
}
