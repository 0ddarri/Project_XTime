using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GAME_STATE { INTRO, INGAME, PAUSED, RESULT }

public class BaseScene : MonoBehaviour
{
    [SerializeField] GAME_STATE State;
    [SerializeField] IntroUIManager IntroUIManager;
    public MapManager MapManager;
    public BuildingManager buildingManager;
    public UnitManager UnitManager;
    public Transform TrashParent;
    public TrashManager TrashManager;
    public MoneyController MoneyController;

    [Space(5.0f)]
    [SerializeField] EndingUIController EndingUI;
    [SerializeField] float ConfidenceMinValue;

    [Space(5.0f)]
    public float SaveTime = 0.0f;
    [SerializeField] float MaxSaveTime = 0.0f;
    [SerializeField] Text MaxSaveTimeText;

    private void Awake()
    {
        SceneManager.Ins.Scene = this;
        ChangeState(GAME_STATE.INTRO);
    }

    public bool IsState(GAME_STATE state)
    {
        return State == state ? true : false;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
        IntroUIManager.Initialize();
        MapManager.Initialize();
        EndingUI.gameObject.SetActive(false);
        MoneyController.Initialize();
        IOManager.Ins.Load();
        MaxSaveTimeText.text = "�ְ��� : " + IOManager.Ins.MaxSaveTime.ToString("F1") + "s";
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
        SaveTime = 0.0f;
        Time.timeScale = 1.0f;
        IntroUIManager.Initialize();
        EndingUI.gameObject.SetActive(false);
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
        Time.timeScale = 0.0f;
        EndingUI.gameObject.SetActive(true);
        EndingUI.Initialize();
        if(SaveTime > MaxSaveTime)
        {
            IOManager.Ins.Save();
        }
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
                {
                    SaveTime += Time.deltaTime;
                    if (UnitManager.Confidence < ConfidenceMinValue)
                    {
                        ChangeState(GAME_STATE.RESULT);
                    }
                }
                break;

            case GAME_STATE.PAUSED:
                break;

            case GAME_STATE.RESULT:
                break;
        }
    }
}
