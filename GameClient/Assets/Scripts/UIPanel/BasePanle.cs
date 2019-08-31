using System.Collections;
using UnityEngine;

public class BasePanle : MonoBehaviour {

    protected UIManager uiMng;
    protected GameFocade focade;

    public GameFocade Focade
    {
        set { focade = value; }
    }

    public UIManager UIMng
    {
        set { uiMng = value; }
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnPause()
    {
    }

    public virtual void OnResume()
    {

    }

    public virtual void OnExit()
    {

    }

    public void PlayClickSound()
    {
        focade.PlayNomalSound(AudioManager.sound_ButtonClick);
    }
}
