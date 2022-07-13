using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

public class ClimateManager : MonoBehaviour
{
    [SerializeField] List<LevelClimate> LevelClimateList = new List<LevelClimate>();
    [SerializeField] int CurrentClimateLevel;
    [SerializeField] float CurrentLevelTime = 0.0f;

    [Space(5.0f)]
    [SerializeField] float CurrentClimateDelay = 0.0f;
 
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

            LevelClimateList[CurrentClimateLevel].ClimateDelay = Random.Range(LevelClimateList[CurrentClimateLevel].ClimateDelayMin, LevelClimateList[CurrentClimateLevel].ClimateDelayMax);
        }
    }

    private void Update()
    {
        UpdateLevel();
        UpdateClimate();
    }
}
