using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<BaseBuilding> Buildings = new List<BaseBuilding>();

    public void Initialize()
    {
        for(int i = 0; i < Buildings.Count; i++)
        {
            Buildings[i].Initialize();
        }
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
}
