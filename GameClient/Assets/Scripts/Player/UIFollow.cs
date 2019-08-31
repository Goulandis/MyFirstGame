using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour {

    private Camera Miancamera;
    private string playerName;
    public GameObject player;
    private float height;

    public Texture2D hpForeground;//血条的前景贴图
    public Texture2D hpBackground;//血条的背景贴图
    private int HP;
    private int offset = 30;//位置偏移，计算位置时可能出现有一些误差，偏移消除这些误差

    public Font fort;//显示名字的文字字体样式

    private void Start()
    {
        Miancamera = Camera.main;
        float size_y = player.GetComponent<Collider>().bounds.size.y;//获取游戏对象原始模型的高度值
        float scal_y = transform.localScale.y;//获取游戏对象当前缩放比例
        height = size_y * scal_y;//原始模型高度 * 当前缩放比例 = 模型当前高度
    }

    private void OnGUI()
    {
        //计算绘制血条和名称所在的世界坐标，此坐标就是玩家的世界坐标
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        //血条和名称所在世界坐标转换成屏幕坐标
        Vector2 position = Miancamera.WorldToScreenPoint(worldPosition);
        //计算绘制血条和名称坐在的屏幕坐标，由于屏幕坐标屏幕在右下角是(0,0)点，左上角是(Screen.Width,Screen.Height)所以使用减法
        position = new Vector2(position.x, Screen.height - position.y);

        //计算绘制血条的大小，大小有hpForeground确定，[GUI.skin.lable]-GUIStyle
        Vector2 hpSize = GUI.skin.label.CalcSize(new GUIContent(hpForeground));
        //计算绘制血条的宽度，这样就可以实现扣血时，血条宽度的变化
        int hpWidth = hpForeground.width * HP / 200;
        //绘制血条
        GUI.DrawTexture(new Rect(position.x - hpSize.x / 2, position.y - hpSize.y - offset, hpSize.x, hpSize.y), hpBackground);
        GUI.DrawTexture(new Rect(position.x - hpSize.x / 2 , position.y - hpSize.y - offset, hpWidth, hpSize.y), hpForeground);

        //计算绘制名称的大小，以确定绘制名称的位置
        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(playerName));
        GUI.color = Color.black;
        //设置字体样式
        GUI.skin.label.font = fort;
        //绘制名称
        GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y - hpSize.y - offset, nameSize.x, nameSize.y), playerName);
    }

    public void SetUserName(string name)
    {
        playerName = name;
    }

    public void SetHP(int HP)
    {
        this.HP = HP;
    }
}
