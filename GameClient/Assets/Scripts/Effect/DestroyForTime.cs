using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//销毁物体，目前只用在了销毁箭矢碰撞特效的销毁
public class DestroyForTime : MonoBehaviour
{
    public int time = 1;

	void Start () {
		Destroy(this.gameObject,time);
	}
}
