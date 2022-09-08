using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public enum ENV_TYPE
{
    NONE, // 기본기후
    MIST, // 안개
    YELLOW_DUST, // 황사
    TORNADO, // 토네이도
    RAIN, // 비
    SNOW, // 눈
    QUAKE // 지진
}

[System.Serializable]
public class LevelClimate // 단계별 이상기후 리스트
{
    public List<BaseClimate> ClimateList = new List<BaseClimate>();
    public float LevelTime;
    public float ClimateDelayMin;
    public float ClimateDelayMax;
    public float ClimateDelay;


    public void GetRandomClimate(ref BaseClimate climate)
    {
        int RandomNumber = Random.Range(0, ClimateList.Count);
        climate = ClimateList[RandomNumber];
    }

    public void Initialize()
    {
        for(int i = 0; i < ClimateList.Count; i++)
            ClimateList[i].Initialize();
    }
}

public class ClimateManager : MonoBehaviour
{
    public Volume GlobalVolume;
    public Material BackgroundMat;
    [SerializeField] protected Color TopColor;
    [SerializeField] protected Color MiddleColor;
    [SerializeField] protected Color BottomColor;
    public Image BottomGradientSprite;
    [Space(5.0f)]
    [SerializeField] List<LevelClimate> LevelClimateList = new List<LevelClimate>();
    [SerializeField] int CurrentClimateLevel;
    [SerializeField] float CurrentLevelTime = 0.0f;

    [Space(5.0f)]
    [SerializeField] float CurrentClimateDelay = 0.0f;

    public void Initialize()
    {
        for(int i = 0; i < LevelClimateList.Count; i++)
            LevelClimateList[i].Initialize();

        BackgroundMat.SetColor("_TopColor", TopColor);
        BackgroundMat.SetColor("_MiddleColor", MiddleColor);
        BackgroundMat.SetColor("_BottomColor", BottomColor);
        BottomGradientSprite.color = Color.white;
    }

    private void Start()
    {
        Initialize();
    }

    public IEnumerator SetGraphicsEnd()
    {
        float time = 0.0f;

        Color originTopColor = BackgroundMat.GetColor("_TopColor");
        Color originMiddleColor = BackgroundMat.GetColor("_MiddleColor");
        Color originBottomColor = BackgroundMat.GetColor("_BottomColor");

        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            BackgroundMat.SetColor("_TopColor", Color.Lerp(originTopColor, TopColor, time));
            BackgroundMat.SetColor("_MiddleColor", Color.Lerp(originMiddleColor, MiddleColor, time));
            BackgroundMat.SetColor("_BottomColor", Color.Lerp(originBottomColor, BottomColor, time));

            yield return null;
        }
    }

    void UpdateLevel() // 현재 이상기후 단계 계산
    {
        CurrentLevelTime += Time.deltaTime;

        for(int i = 0; i < LevelClimateList.Count; i++)
        {
            if(LevelClimateList[i].LevelTime < CurrentLevelTime)
                CurrentClimateLevel = i;
        }
    }

    bool CheckClimatePlaying()
    {
        for (int i = 0; i < LevelClimateList.Count; i++)
        {
            for (int j = 0; j < LevelClimateList[i].ClimateList.Count; j++)
            {
                if (LevelClimateList[i].ClimateList[j].IsPlaying)
                {
                    CurrentClimateDelay = 0.0f;
                    return true;
                }
            }
        }
        return false;
    }

    void UpdateClimate()
    {
        if (CheckClimatePlaying())
            return;
        CurrentClimateDelay += Time.deltaTime;
        if(CurrentClimateDelay > LevelClimateList[CurrentClimateLevel].ClimateDelay)
        {
            BaseClimate climate = new BaseClimate();
            LevelClimateList[CurrentClimateLevel].GetRandomClimate(ref climate);
            climate.StartClimate();
            Invoke("IV_SetBGOrigin", climate.RandomTime);
            LevelClimateList[CurrentClimateLevel].ClimateDelay = Random.Range(LevelClimateList[CurrentClimateLevel].ClimateDelayMin, LevelClimateList[CurrentClimateLevel].ClimateDelayMax);
        }
    }

    void IV_SetBGOrigin()
    {
        StartCoroutine(SetGraphicsEnd());
    }

    private void Update()
    {
        if (SceneManager.Ins.Scene.IsState(GAME_STATE.INTRO))
            return;

        UpdateLevel();
        UpdateClimate();
    }
}
