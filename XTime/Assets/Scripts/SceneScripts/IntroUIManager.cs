using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroUIManager : MonoBehaviour
{
    [SerializeField] IsoButton StartButton;
    [SerializeField] IsoButton SettingsButton;
    [SerializeField] IsoButton ExitButton;

    [Header("Animation")]
    [SerializeField] PlayableDirector GameStartAnimation;

    public void Initialize()
    {
        StartButton.enabled = false;
        SettingsButton.enabled = false;
        ExitButton.enabled = false;
    }
    
    public void UIUpdate()
    {
        if(StartButton.IsClicked)
        {
            StartButton.IsClicked = false;
            Initialize();
            SceneManager.Ins.Scene.ChangeState(GAME_STATE.INGAME);
            GameStartAnimation.Play();
            Debug.Log("Start");
        }

        if (SettingsButton.IsClicked)
        {
            SettingsButton.IsClicked = false;
            Initialize();
            SceneManager.Ins.Scene.ChangeState(GAME_STATE.PAUSED);
            Debug.Log("Settings");
        }

        if (ExitButton.IsClicked)
        {
            ExitButton.IsClicked = false;
            Initialize();
            Application.Quit();
            Debug.Log("Exit");
        }
    }

}
