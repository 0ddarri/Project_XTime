using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public List<TrashObject> TrashList = new List<TrashObject>();

    public void ClearTrash()
    {
        for(int i = 0; i < TrashList.Count; i++)
        {
            Destroy(TrashList[i].gameObject);
        }
        TrashList.Clear();

        SceneManager.Ins.Scene.buildingManager.ClearCompany();
        Debug.Log("쓰레기 클리어");
    }
}
