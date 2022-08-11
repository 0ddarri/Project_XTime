using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    [SerializeField] List<Entrance> EntranceList = new List<Entrance>();
    public int Index;

    public void Initialize()
    {
        for(int i = 0; i < EntranceList.Count; i++)
        {
            EntranceList[i].Initialize();
        }
    }

    public Entrance GetRandomEntrance()
    {
        int random = Random.Range(0, EntranceList.Count);
        return EntranceList[random];
    }

    public void SetEntranceActive(bool value)
    {
        for (int i = 0; i < EntranceList.Count; i++)
        {
            EntranceList[i].gameObject.SetActive(false);
        }
    }

    void CheckEnter()
    {
        for(int i = 0; i < EntranceList.Count; i++)
        {
            if(EntranceList[i].IsEnter)
            {
                Initialize();
            }
        }
    }

    private void Update()
    {
        CheckEnter();
    }
}
