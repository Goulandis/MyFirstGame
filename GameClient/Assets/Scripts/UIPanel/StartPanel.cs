using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanle {

    private Button startGameButton;
    private Animator btnAnimator;

    public override void OnEnter()
    {
        base.OnEnter();
        startGameButton = transform.Find("StartGameButton").GetComponent<Button>();
        btnAnimator = startGameButton.GetComponent<Animator>();
        startGameButton.onClick.AddListener(OnLoginClick);
    }

    private void OnLoginClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Login);
    }

    public override void OnPause()
    {
        base.OnPause();
        btnAnimator.enabled = false;
        Tweener tweener = startGameButton.transform.DOScale(0, 0.4f);
        tweener.OnComplete(() => startGameButton.gameObject.SetActive(false));
        gameObject.SetActive(false);

    }

    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
        startGameButton.gameObject.SetActive(true);
        startGameButton.transform.DOScale(1, 0.4f).OnComplete(()=> btnAnimator.enabled = true);
    }
}
