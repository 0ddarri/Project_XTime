using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IOManager : Singleton<IOManager>
{
    public float MaxSaveTime = 0.0f;

    public void Save()
    {
        PlayerPrefs.SetFloat("SaveTime", SceneManager.Ins.Scene.SaveTime);
        Debug.Log("Save");
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("SaveTime"))
        {
            MaxSaveTime = PlayerPrefs.GetFloat("SaveTime");
            Debug.Log("Load");
        }
    }
}
