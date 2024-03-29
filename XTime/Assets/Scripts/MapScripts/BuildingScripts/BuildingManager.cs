using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] float BadEmotion = 0.0f;
    [SerializeField] Transform BadEmotionUI;
    [SerializeField] bool IsDecreaseEmotion;

    public QuestController QuestController;
    [SerializeField] float QuestDelayMin; 
    [SerializeField] float QuestDelayMax; 
    [SerializeField] float CurrentQuestDelay; 
    [SerializeField] float QuestDelayTime = 0.0f;

    [Header("Broke Settings")]
    [SerializeField] IsoButton FixButton;
    [SerializeField] int FixCost;
    
    public bool DecreaseAvail
    {
        get
        {
            return IsDecreaseEmotion;
        }
        set
        {
            IsDecreaseEmotion = value;
            if (IsDecreaseEmotion)
                StartCoroutine(DecreaseEmotion());
        }
    }


    public float Emotion
    {
        get
        {
            return BadEmotion;
        }
        set
        {
            BadEmotion = value;
            if(BadEmotion > 1.0f)
                BadEmotion = 1.0f;
            if (BadEmotion < 0.0f)
                BadEmotion = 0.0f;
        }
    }

    void SetEmotionUI()
    {
        Vector3 scale = BadEmotionUI.localScale;
        BadEmotionUI.localScale = new Vector3(Emotion, scale.y, scale.z);
    }

    IEnumerator DecreaseEmotion()
    {
        while(DecreaseAvail)
        {
            yield return new WaitForSeconds(60.0f);
            Emotion -= 0.1f;
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        for(int i = 0; i < Buildings.Count; i++)
        {
            Buildings[i].Initialize();
        }
        CurrentCompanyTime = 0.0f;
        Emotion = 0.0f;
        DecreaseAvail = true;
        QuestDelayTime = 0.0f;
        CurrentQuestDelay = Random.Range(QuestDelayMin, QuestDelayMax);
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
    
    public List<BaseBuilding> GetRandomBuilding(int count)
    {
        List<BaseBuilding> randBuildings = new List<BaseBuilding>();
        List<int> checkSamelist = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int n = Random.Range(0, Buildings.Count);
            if(CheckSame(checkSamelist, n))
            {
                i--;
                continue;
            }
            checkSamelist.Add(n);
            randBuildings.Add(Buildings[n]);
        }
        return randBuildings;
    }

    bool CheckSame(List<int> checklist, int num)
    {
        for(int i = 0; i < checklist.Count; i++)
        {
            if (checklist[i] == num)
                return true;
        }

        return false;
    }

    public void SetCompany()
    {
        CurrentCompanyTime += Time.deltaTime;
        if(CurrentCompanyTime > CompanyTime)
        {
            CurrentCompanyTime = 0.0f;
            CompanyTime = Random.Range(CompanyTimeMin, CompanyTimeMax);
            int random = Random.Range(0, Buildings.Count);
            if (Buildings[random].Broken)
                return;

            Buildings[random].Company = true;
        }
    }

    bool CheckCompany()
    {
        for(int i = 0; i < Buildings.Count; i++)
        {
            if (Buildings[i].Company)
                return true;
        }
        return false;
    }

    public int CheckBreakBuildingCount()
    {
        int result = 0;
        for(int n = 0; n < Buildings.Count; n++)
        {
            if (Buildings[n].Broken)
                result++;
        }
        return result;
    }

    public void ClearCompany()
    {
        for(int i = 0; i < Buildings.Count; i++)
        {
            Buildings[i].Company = false;
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

        FixCost = CheckBreakBuildingCount() * 2;

        SetCompany();

        if (CheckCompany() && !QuestController.QuestAvail)
        {
            QuestDelayTime += Time.deltaTime;
            if(QuestDelayTime > CurrentQuestDelay)
            {
                QuestDelayTime = 0.0f;
                if (!QuestController.QuestAvail)
                    QuestController.QuestAvail = true;
                CurrentQuestDelay = Random.Range(QuestDelayMin, QuestDelayMax);
            }
        }
        else
            QuestDelayTime = 0.0f;

        SetEmotionUI();

        if(FixButton.IsClicked)
        {
            FixButton.IsClicked = false;
            if(SceneManager.Ins.Scene.MoneyController.Money >= FixCost)
            {
                SceneManager.Ins.Scene.MoneyController.Money -= FixCost;
                FixAllBuilding();
            }
        }
    }

    public void FixAllBuilding()
    {
        for(int i = 0; i < Buildings.Count; i++)
        {
            if (Buildings[i].Broken)
                Buildings[i].Broken = false;
        }
    }
}
