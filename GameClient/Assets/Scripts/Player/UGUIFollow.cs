using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//角色头顶UI-名字，血条的动态显示
public class UGUIFollow : MonoBehaviour {
    private Camera mainCamera;
    public GameObject namePanel;
    private Text playerName;
    private Image HpObj;
    private GameObject damageObj;

    private float initHp;
    private float HP;
    private int damage = 0;
    private bool isSetHp = false;

	void Start () {
        mainCamera = Camera.main;
        playerName = transform.Find("Name").GetComponent<Text>();
        HpObj = transform.Find("HP/Fill Area/Fill").GetComponent<Image>();
        damageObj = GameObject.Find("Damage");
        damageObj.SetActive(false);
    }
	
	void Update () {
        namePanel.transform.rotation = mainCamera.transform.rotation;
        if (isSetHp)
        {
            HpObj.fillAmount = HP / initHp;
            if (damage != 0)
            {
                damageObj.SetActive(true);
                ShowDamage();
                damage = 0;
            }

            isSetHp = false;
        }
	}

    public void SetUserName(string name)
    {
        if (playerName == null)
        {
            playerName = transform.Find("Name").GetComponent<Text>();
        }
        playerName.text = name;

    }

    public void SetHPAndDamage(int HP,int damage)
    {
        this.HP = HP;
        this.damage = damage;
        isSetHp = true;
    }

    public void SetHPAndDamage(int HP)
    {
        this.HP = HP;
    }

    public void InitHp(int HP)//初始化血量值
    {
        if (HpObj == null)
        {
            HpObj = transform.Find("HP/Fill Area/Fill").GetComponent<Image>();
        }
        initHp = HP;
        HpObj.fillAmount = 1;
    }

    public void ShowDamage()//伤害漂浮显示
    {
        damageObj.SetActive(true);
        Text damageValue = damageObj.transform.GetComponent<Text>();
        damageValue.text = damage.ToString();
        damageObj.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
        Color tempColor = damageValue.color;
        tempColor.a = 1;
        damageValue.color = tempColor;
        damageValue.transform.DOScale(0.03f, 0.3f).SetDelay(0.3f);
        damageValue.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => damageValue.gameObject.SetActive(false));
    }
}

