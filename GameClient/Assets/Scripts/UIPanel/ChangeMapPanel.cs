using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Map;
using Common;

public class ChangeMapPanel : BasePanle {

    private Image map;
    private Button changeMapButton;
    private Button goLift;
    private Button goRight;
    private Text mapNameStr;
    private Button closeButton;
    private List<Sprite> mapArray = new List<Sprite>();
    private List<string> mapName = new List<string>();

    public int currentMap = 0;//地图索引
    private int currentMapCount = 4;

    private CreateRoomRequest createRoomRequest;

    void Start () {
        //加载地图预览到地图数组
        for (int i = 0; i < currentMapCount; i++) {
            mapArray.Add(new MapInfo(i,RoleType.None).MapSprite);
            mapName.Add(new MapInfo(i,RoleType.None).MapName);
        }

        mapNameStr = transform.Find("Foreground/MapName").GetComponent<Text>();
        map = transform.Find("Foreground/Map").GetComponent<Image>();
        map.sprite = mapArray[currentMap];
        mapNameStr.text = mapName[currentMap];

        goLift = transform.Find("Foreground/GoLift").GetComponent<Button>();
        goRight = transform.Find("Foreground/GoRight").GetComponent<Button>();
        closeButton = transform.Find("Foreground/Close").GetComponent<Button>();
        changeMapButton = transform.Find("Foreground/Map").GetComponent<Button>();
        goLift.onClick.AddListener(OnGoLiftMapClick);
        goRight.onClick.AddListener(OnGoRightMapClick);
        closeButton.onClick.AddListener(OnCloseClick);
        changeMapButton.onClick.AddListener(OnChangeMapClick);

        createRoomRequest = GetComponent<CreateRoomRequest>();
    }

	void Update () {
        if (currentMap == 0)
        {
            goLift.gameObject.SetActive(false);
        }
        else if (currentMap == mapArray.Count - 1)
        {
            goRight.gameObject.SetActive(false);
        }
        else
        {
            goLift.gameObject.SetActive(true);
            goRight.gameObject.SetActive(true);
        }
	}

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.4f);
    }

    public override void OnResume()
    {
        base.OnResume();
        OnEnter();
    }

    public override void OnPause()
    {
        HideAnime();
    }

    public override void OnExit()
    {
        HideAnime();
    }

    private void HideAnime()
    {
        transform.DOScale(0, 0.4f);
        transform.DOLocalMoveX(-1000, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnGoLiftMapClick()
    {
        if (currentMap > 0)
        {
            currentMap--;
            map.sprite = mapArray[currentMap];
            mapNameStr.text = mapName[currentMap];
        }
    }
    private void OnGoRightMapClick()
    {
        if (currentMap < mapArray.Count - 1)
        {           
            currentMap++;
            map.sprite = mapArray[currentMap];
            mapNameStr.text = mapName[currentMap];
        }
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }

    private void OnChangeMapClick()
    {
        PlayClickSound();
        BasePanle panel = uiMng.PushPanel(UIPanelType.Room);
        createRoomRequest.SetPanel(panel);
        createRoomRequest.SendRequest(currentMap.ToString());//发送地图索引给服务器
    }

}
