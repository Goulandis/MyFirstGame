using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityStandardAssets.ImageEffects;

//管理相机
public class CameraManager : BaseManager
{
    private GameObject cameraGo;
    private Animator cameraAnime;
    private FollowRole followRole;
    private TiltShift tiltShift;
    private VignetteAndChromaticAberration vigChrAbe;
    private ColorCorrectionLookup clrCorLookup;
    private GameObject rain;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    public CameraManager(GameFocade facade) : base(facade)
    {

    }

    public override void OnInit()
    {
        cameraGo = Camera.main.gameObject;
        cameraAnime = cameraGo.GetComponent<Animator>();
        followRole = cameraGo.GetComponent<FollowRole>();
        tiltShift = cameraGo.GetComponent<TiltShift>();
        vigChrAbe = cameraGo.GetComponent<VignetteAndChromaticAberration>();
        clrCorLookup = cameraGo.GetComponent<ColorCorrectionLookup>();
        rain = GameObject.FindGameObjectWithTag("EffectRain");
    }

    public void FollowRole()
    {
        //Camera.main.orthographic = true;
        rain.gameObject.SetActive(false);
        tiltShift.enabled = false;
        vigChrAbe.enabled = false;
        clrCorLookup.enabled = false;
        //设置Camera要跟随的Position，不同的客户端Position不同，所以须要动态获取Position
        followRole.playerTrans = facade.GetCurrenRoleGameObject().transform;
        cameraAnime.enabled = false;//禁用Animator，防止读取position和eulerAngles时，Camera还在运动
        originalPosition = cameraGo.transform.position;//记录Camera当前的在Animator中运动到的位置
        originalRotation = cameraGo.transform.eulerAngles;//记录Camera当前的在Animator中运动到的角度
        //获取Camera当前位置到Player位置的先转角
        Quaternion targetQuaternion = Quaternion.LookRotation(followRole.playerTrans.position-cameraGo.transform.position);
        //启用DOTweening动画将Animator中的Camera过渡到FollowTarget中
        followRole.enabled = true;
        cameraGo.transform.DORotateQuaternion(targetQuaternion, 1);
    }

    //将FollowTarget中的Camera过渡到Animator中
    public void WalkthroughScene()
    {
        //Camera.main.orthographic = false;
        rain.gameObject.SetActive(true);
        tiltShift.enabled = true;
        vigChrAbe.enabled = true;
        clrCorLookup.enabled = true;
        followRole.enabled = false;
        cameraGo.transform.DOMove(originalPosition, 1);
        cameraGo.transform.DORotate(originalRotation, 1).OnComplete(() => cameraAnime.enabled = true);
    }
}
