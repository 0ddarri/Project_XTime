using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GAME_STATE { INTRO, INGAME, PAUSED, RESULT }

public class BaseScene : MonoBehaviour
{
    [SerializeField] GAME_STATE State;
    public IntroUIManager IntroUIManager;
    public ClimateManager ClimateManager;
    public MapManager MapManager;
    public BuildingManager buildingManager;
    public UnitManager UnitManager;
    public Transform TrashParent;
    public TrashManager TrashManager;
    public MoneyController MoneyController;
    public SettingsUIController SettingsUIController;
    public SoundManager SoundManager;
    [SerializeField] GameObject TutorialUI;

    [Space(5.0f)]
    [SerializeField] EndingUIController EndingUI;
    [SerializeField] float ConfidenceMinValue;

    [Space(5.0f)]
    public float SaveTime = 0.0f;
    [SerializeField] float MaxSaveTime = 0.0f;
    [SerializeField] Text MaxSaveTimeText;
    [SerializeField] LanguageString SaveTimeString;
    string OriginStr;

    private void Awake()
    {
        SceneManager.Ins.Scene = this;
        ChangeState(GAME_STATE.INTRO);
        IOManager.Ins.Initialize();
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
        SettingsUIController.Load();
        MaxSaveTimeText.text = SaveTimeString.str + " : " + IOManager.Ins.MaxSaveTime.ToString("F1") + "s";
        OriginStr = SaveTimeString.str;
        SoundManager.PlaySound(SOUND_TYPE.BGM, "Main BGM"); 
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
        TutorialUI.SetActive(true);
    }

    void IngameInit()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(MapManager.StartObjAnimation());
        TutorialUI.SetActive(false);
    }

    void PauseInit()
    {
        SettingsUIController.Initialize();
        Time.timeScale = 0.0f;
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

    void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(GAME_STATE.PAUSED);
        }
    }

    private void Update()
    {
        if(OriginStr != SaveTimeString.str)
        {
            OriginStr = SaveTimeString.str;
            MaxSaveTimeText.text = SaveTimeString.str + " : " + IOManager.Ins.MaxSaveTime.ToString("F1") + "s";
        }

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
                    else
                    {
                        Pause();
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
