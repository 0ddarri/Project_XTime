using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<BaseBuilding> Buildings = new List<BaseBuilding>();
    [SerializeField] float CompanyTimeMin = 0.0f;
    [SerializeField] float CompanyTimeMax = 0.0f;
    [SerializeField] float CompanyTime = 0.0f;
    [SerializeField] float CurrentCompanyTime = 0.0f;

    [Header("Trash")]
    [SerializeField] List<Transform> TrashSpawnList = new List<Transform>();
    public GameObject TrashPrefab;

    public void Initialize()
    {
        for(int i = 0; i < Buildings.Count; i++)
        {
            Buildings[i].Initialize();
        }
        CurrentCompanyTime = 0.0f;
    }

    public List<int> GetRandomBuildingNum()
    {
        List<int> num = new List<int>();

        int range = Random.Range(3, 5);

        for(int i = 0; i < range; i++)
        {
            int n = Random.Range(0, Buildings.Count);
            num.Add(n);
        }

        return num;
    }

    public void SetCompany()
    {
        CurrentCompanyTime += Time.deltaTime;
        if(CurrentCompanyTime > CompanyTime)
        {
            CurrentCompanyTime = 0.0f;
            CompanyTime = Random.Range(CompanyTimeMin, CompanyTimeMax);
            int random = Random.Range(0, Buildings.Count);
            Buildings[random].Company = true;
        }
    }

    public Transform GetRandomTrashPos()
    {
        return TrashSpawnList[Random.Range(0, TrashSpawnList.Count)];
    }

    private void Update()
    {
        if (SceneManager.Ins.Scene.IsState(GAME_STATE.INTRO))
            return;

        SetCompany();
    }
}
