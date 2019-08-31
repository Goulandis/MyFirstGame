using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

//专门负责处理箭矢固有属性
public class Arrow : MonoBehaviour
{
    private int speed = 10;//箭矢速度
    public bool isLocal = false;//标识箭矢是否为自身角色的
    public RoleType roleType;//保存箭的类型信息
    private Rigidbody rgb;
    public GameObject explostionEffect;//箭矢爆炸特效

	void Start ()
	{
	    rgb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        //箭矢移动
        rgb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
	}

    //碰撞检测
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameFocade.Instance.PlayNomalSound(AudioManager.sound_ShootPerson);
            if (isLocal)
            {
                bool isLocalPlayer = other.GetComponent<PlayerInfo>().isLocal;
                if (isLocal != isLocalPlayer)
                {
                    GameFocade.Instance.SendAttack(Random.Range(10,20));
                }
            }
        }
        else
        {
            GameFocade.Instance.PlayNomalSound(AudioManager.sound_Miss);
        }
        GameObject.Instantiate(explostionEffect, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
