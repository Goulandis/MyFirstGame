using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Map;
using Common;

public class RoomItem : MonoBehaviour
{
    public int id;
    public Text userName;
    public Text totalCount;
    public Text WinCount;
    public Button joinButton;
    private RoomListPanel roomListPanel;
    private Image mapImage;

    void Start()
    {
        joinButton = transform.Find("Join").GetComponent<Button>();
        mapImage = transform.Find("MapImage").GetComponent<Image>();
        if (joinButton != null)
        {
            joinButton.onClick.AddListener(OnJionClick);
        }
    }
	
	void Update () {
		
	}

    private void OnJionClick()
    {
        roomListPanel.OnJoinClick(id);
    }

    public void SetRoomItemRes(int id,string userName,int totalCount,int winCount,int mapIndex,RoomListPanel roomListPanel)
    {
        this.id = id;
        this.userName.text = userName;
        this.roomListPanel = roomListPanel;
        this.totalCount.text = "总场数\n" + totalCount.ToString();
        this.WinCount.text = "胜利\n" + winCount.ToString();
        if(mapImage == null)
            mapImage = transform.Find("MapImage").GetComponent<Image>();
        mapImage.sprite = new MapInfo(mapIndex, RoleType.None).MapSprite;

    }

    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
