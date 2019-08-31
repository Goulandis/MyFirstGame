using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//专门负责控制角色移动
public class PlayerMove : MonoBehaviour
{
    private float speed = 4f;//角色移动速度
    private Animator anime;
    private float h, v;

    public float forward = 0;//用于动画同步

	private void Start ()
	{
	    anime = GetComponent<Animator>();
	}
	
    //本例中角色动画对坐标无影响，角色移动由代码控制
	private void Update ()
	{
	    if (anime.GetCurrentAnimatorStateInfo(0).IsName("Grounded") == false)
            return;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)//防止没有转向时transform.rotation循环输入0
        {
            transform.Translate(new Vector3(h, 0, v)* Time.deltaTime * speed, Space.World);//角色移动
            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));//角色转向

            float res = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
            forward = res;
            anime.SetFloat("Forward", res);//通过状态机Parameters变量控制角色移动动画
        }
    }
}
