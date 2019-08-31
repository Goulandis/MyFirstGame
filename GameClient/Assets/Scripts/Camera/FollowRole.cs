using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//相机跟随
public class FollowRole : MonoBehaviour
{
    public Transform playerTrans;
    private Vector3 offset = new Vector3(-1, 11, -9);//固定偏移地址
    private float smoothing = 2;

    void Update()
    {
        Vector3 targetposition = playerTrans.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetposition, smoothing * Time.deltaTime);
        transform.LookAt(playerTrans);
    }
}
