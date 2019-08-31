using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuitPanel : BasePanle {

    private Button sureButton;
    private Button cancleButton;

	void Start () {
        sureButton = transform.Find("SureButton").GetComponent<Button>();
        cancleButton = transform.Find("CancleButton").GetComponent<Button>();
        sureButton.onClick.AddListener(Sure);
        cancleButton.onClick.AddListener(Cancel);
	}
	
	void Update () {
		
	}

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
        transform.DOScale(1, 0.4f);
    }

    public override void OnExit()
    {
        base.OnExit();
        Tweener tweener = transform.DOScale(0, 0.4f);
        tweener.OnComplete(() => gameObject.SetActive(false));

    }

    private void Sure()
    {
        Application.Quit();
    }

    private void Cancel()
    {
        uiMng.PopPanel();
    }
}
