using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE { INTRO, INGAME, PAUSED, RESULT }

public class BaseScene : MonoBehaviour
{
    [SerializeField] GAME_STATE State;
    [SerializeField] IntroUIManager IntroUIManager;
    public MapManager MapManager;
    public BuildingManager buildingManager;

    private void Awake()
    {
        SceneManager.Ins.Scene = this;
        ChangeState(GAME_STATE.INTRO);
    }

    private void Start()
    {
        IntroUIManager.Initialize();
        MapManager.Initialize();
    }

    public void ChangeState(GAME_STATE newState)
    {
        if (State == newState)
            return;

        State = newState;
        switch(State)
        {
            case GAME_STATE.INTRO:
                IntroInit();
                break;

            case GAME_STATE.INGAME:
                IngameInit();
                break;

            case GAME_STATE.PAUSED:
                PauseInit();
                break;

            case GAME_STATE.RESULT:
                ResultInit();
                break;
        }
    }

    void IntroInit()
    {
        IntroUIManager.Initialize();
    }

    void IngameInit()
    {
        StartCoroutine(MapManager.StartObjAnimation());
    }

    void PauseInit()
    {

    }

    void ResultInit()
    {

    }

    private void Update()
    {
        switch (State)
        {
            case GAME_STATE.INTRO:
                {
                    IntroUIManager.UIUpdate();
                }
                break;

            case GAME_STATE.INGAME:
                break;

            case GAME_STATE.PAUSED:
                break;

            case GAME_STATE.RESULT:
                break;
        }
    }
}
