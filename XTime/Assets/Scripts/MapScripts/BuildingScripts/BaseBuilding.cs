using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    [SerializeField] List<Entrance> EntranceList = new List<Entrance>();
    public int Index;
    [SerializeField] bool IsCompany = false;
    [SerializeField] bool IsBroken = false;
    public bool Broken
    {
        get
        {
            return IsBroken;
        }
        set
        {
            IsBroken = value;
            if(IsBroken)
            {
                Sprite.enabled = false;
            }
            else
            {
                Sprite.enabled = true;
            }
        }
    }

    [Header("Company Settings")]
    [SerializeField] float TrashSpawnMin;
    [SerializeField] float TrashSpawnMax;
    [SerializeField] float CurTrashTime;
    [SerializeField] float CurTrashSpawn;
    [Space(5.0f)]
    [SerializeField] float CompanyTime;
    [SerializeField] float CurCompanyTime = 0.0f;
    [Space (5.0f)]
    [SerializeField] SpriteRenderer Sprite;

    private void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }


    public bool Company
    {
        get
        {
            return IsCompany;
        }

        set
        {
            IsCompany = value;
            if (!IsCompany)
            {
                Sprite.color = Color.white;
                CurTrashTime = 0.0f;
            }
            else
            {
                Sprite.color = new Color(0.1f, 0.1f, 0.1f);
                CurCompanyTime = 0.0f;
            }
        }
    }

    public void Initialize()
    {
        for(int i = 0; i < EntranceList.Count; i++)
        {
            EntranceList[i].Initialize();
        }
        CurTrashSpawn = Random.Range(TrashSpawnMin, TrashSpawnMax);
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
                for (int j = 0; j < EntranceList.Count; j++)
                {
                    EntranceList[j].Initialize();
                }
            }
        }
    }

    void UpdateCompany()
    {
        if (!Company)
            return;

        CurCompanyTime += Time.deltaTime;
        if (CurCompanyTime > CompanyTime)
            Company = false;

        CurTrashTime += Time.deltaTime;
        if(CurTrashTime > CurTrashSpawn)
        {
            CurTrashTime = 0;
            CurTrashSpawn = Random.Range(TrashSpawnMin, TrashSpawnMax);
            SpawnTrash();
        }
    }

    void SpawnTrash()
    {
        BuildingManager manager = SceneManager.Ins.Scene.buildingManager;
        GameObject obj = manager.TrashPrefab;
        Transform transform = manager.GetRandomTrashPos();

        GameObject trashobj = Instantiate(obj, transform.position, Quaternion.identity, SceneManager.Ins.Scene.TrashParent);
        SceneManager.Ins.Scene.TrashManager.TrashList.Add(trashobj.GetComponent<TrashObject>());
    }

    private void Update()
    {
        if (SceneManager.Ins.Scene.IsState(GAME_STATE.INTRO))
            return;

        CheckEnter();
        UpdateCompany();
    }
}
