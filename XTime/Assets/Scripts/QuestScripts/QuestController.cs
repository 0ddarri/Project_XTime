using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
    [SerializeField] bool IsQuestAvail;
    [Space(5.0f)]
    [SerializeField] Quest Quest;

    public bool QuestAvail
    {
        get { return IsQuestAvail; }
        set
        {
            IsQuestAvail = value;
            if(IsQuestAvail)
            {
                Quest.QuestInit();
                Debug.Log("Äù½ºÆ® ¿Â");
            }
        }
    }
}
