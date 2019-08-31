using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//专门负责处理攻击行为
public class PlayerAttack : MonoBehaviour
{
    private Animator anime;
    public GameObject arrow;
    private Transform shootTrans;
    private Vector3 dir;
    private PlayerManager playerMng;

	void Start ()
	{
	    anime = GetComponent<Animator>();
	    shootTrans = transform//实例化箭矢位置
	        .Find(
	            "Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/Bip001 R Hand Prop/shoot")
	        .GetComponent<Transform>();
	}
	
	void Update ()
	{
	    if (anime.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
	    {
	        if (Input.GetMouseButtonDown(0))//鼠标左键攻击
	        {
	            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	            RaycastHit hit;            
                bool isCollider = Physics.Raycast(ray, out hit);
	            if (isCollider)
	            {
	                Vector3 targetPoint = hit.point;
	                targetPoint.y = transform.position.y;
	                dir = targetPoint - transform.position;
                    transform.rotation = Quaternion.LookRotation(dir);
	                anime.SetTrigger("Attack");
                    Invoke("Shoot",0.5f);
                    
	            }
	        }

            if (Input.GetKeyDown(KeyCode.J))//键盘J键攻击
            {
                anime.SetTrigger("Attack");
                Invoke("Shoot", 0.5f);
            }
        }
	}

    public void SetPlayerManager(PlayerManager playerMng)
    {
        this.playerMng = playerMng;
    }

    private void Shoot()
    {
        playerMng.LocalShoot(arrow, shootTrans.position,Quaternion.LookRotation(dir));
    }
}
