using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using Common;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RoomListPanel : BasePanle
{

    private RectTransform battleRes;
    private RectTransform roomList;
    private Button closeButton;
    private Button jionButton;
    private Button createRoomButton;
    private Button refreshButton;
    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPref;

    private List<UserData> userDataList = null;
    private ListRoomRequest listRoomRequest;
    private JoinRoomRequest joinRoomRequest;

    private UserData userData1 = null;
    private UserData userData2 = null;

    private void Start()
    {
        battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        closeButton = transform.Find("RoomList/CloseButton").GetComponent<Button>();
        createRoomButton = transform.Find("RoomList/CreateRoomButton").GetComponent<Button>();
        refreshButton = transform.Find("RoomList/RefreshButton").GetComponent<Button>();
        roomLayout = transform.Find("RoomList/ScrollRect/ListRes").GetComponent<VerticalLayoutGroup>();
        roomItemPref = Resources.Load("UIPanel/RoomItem") as GameObject;
        closeButton.onClick.AddListener(OnCloseClick);
        createRoomButton.onClick.AddListener(OnCreateRoomClick);
        refreshButton.onClick.AddListener(OnRefreshClick);

        listRoomRequest = GetComponent<ListRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();
         
        EnterAnime();
    }

    public void Update()
    {
        if (userDataList != null)
        {
            LoadRoomItem(userDataList);
            userDataList = null;
        }

        if (userData1 != null && userData2 != null)
        {
            BasePanle panel = uiMng.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetAllPlayerResSync(userData1, userData2);
            userData1 = null;
            userData2 = null;
        }
    }

    public override void OnEnter()
    {
        SetBattleRes();
        if (battleRes != null)   
            EnterAnime();
        if (listRoomRequest == null)
            listRoomRequest = GetComponent<ListRoomRequest>();
        listRoomRequest.SendRequest();
    }

    public override void OnPause()
    {       
        HideAnime();
    }

    public override void OnResume()
    {
        EnterAnime();
        listRoomRequest.SendRequest();
    }

    public override void OnExit()
    {
        HideAnime();
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }

    private void OnCreateRoomClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.ChangeMap);
    }

    private void OnRefreshClick()
    {
        PlayClickSound();   
        listRoomRequest.SendRequest();
    }

    private void EnterAnime()
    {
        gameObject.SetActive(true);
        battleRes.localPosition = new Vector3(-1000, 0);
        battleRes.DOLocalMoveX(-285, 0.4f);
        roomList.localPosition = new Vector3(1000, 0);
        roomList.DOLocalMoveX(108, 0.4f);
    }

    private void HideAnime()
    {
        battleRes.DOMoveX(-1000, 0.4f);
        roomList.DOMoveX(-1000, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }

    private void SetBattleRes()
    {
        UserData userData = focade.GetLocalUserData();
        transform.Find("BattleRes/UserName").GetComponent<Text>().text = userData.UserName;
        transform.Find("BattleRes/TotalCount").GetComponent<Text>().text = "总场数\n" + userData.TotalCount.ToString();
        transform.Find("BattleRes/WinCount").GetComponent<Text>().text = "胜利\n" + userData.WinCount.ToString();
    }

    private void LoadRoomItem(List<UserData> useDataList)
    {
        RoomItem[] roomItemList = roomLayout.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem item in roomItemList)
        {
            item.DestroySelf();
        }
        int count = userDataList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject roomItem = GameObject.Instantiate(roomItemPref);
            roomItem.transform.SetParent(roomLayout.transform);
            UserData userData = userDataList[i];
            roomItem.GetComponent<RoomItem>().SetRoomItemRes(userData.Id,userData.UserName,userData.TotalCount,userData.WinCount,userData.MapIndex,this);
        }

        int roomCount = GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            roomCount*(roomItemPref.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing));
    }

    public void LoadRoomItemSync(List<UserData> useDataList)
    {
        this.userDataList = useDataList;
    }

    public void OnJoinClick(int id)
    {
        PlayClickSound();
        joinRoomRequest.SendRequest(id);
    }

    public void OnJoinResponse(ReturnCode returnCode,UserData localUserData,UserData EnemyUserData)
    {
        switch (returnCode)
        {
            case ReturnCode.NotFound:
                uiMng.ShowMessageSync("房间已解散");
                break;
            case ReturnCode.Fail:
                uiMng.ShowMessageSync("房间已满员");
                break;
            case ReturnCode.Success:
                this.userData1 = localUserData;
                this.userData2 = EnemyUserData;
                break;
        }
    }


    public void OnUpdateResultResponse(int totalCount,int winCount)
    {
        focade.UpdateResult(totalCount, winCount);
        SetBattleRes();
    }
}
