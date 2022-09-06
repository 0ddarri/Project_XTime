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

    public void Initialize(bool enable = true)
    {
        StartButton.enabled = enable;
        SettingsButton.enabled = enable;
        ExitButton.enabled = enable;

        StartButton.GetComponent<PolygonCollider2D>().enabled = enable;
        SettingsButton.GetComponent<PolygonCollider2D>().enabled = enable;
        ExitButton.GetComponent<PolygonCollider2D>().enabled = enable;
    }
    
    public void UIUpdate()
    {
        if(StartButton.IsClicked)
        {
            StartButton.IsClicked = false;
            Initialize(false);
            SceneManager.Ins.Scene.ChangeState(GAME_STATE.INGAME);
            GameStartAnimation.Play();
            Debug.Log("Start");
        }

        if (SettingsButton.IsClicked)
        {
            SettingsButton.IsClicked = false;
            Initialize(false);
            SceneManager.Ins.Scene.SettingsUIController.Initialize();
            Debug.Log("Settings");
        }

        if (ExitButton.IsClicked)
        {
            ExitButton.IsClicked = false;
            Initialize(false);
            Application.Quit();
            Debug.Log("Exit");
        }
    }

}
