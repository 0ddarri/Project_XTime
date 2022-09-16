using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IOManager : Singleton<IOManager>
{
    public float MaxSaveTime = 0.0f;

    public int BGM = 1;
    public int SFX = 1;
    public int LANG = 1; // 0 : KOR, 1 : ENG

    public void Initialize()
    {
        if(!PlayerPrefs.HasKey("BGM"))
        {
            PlayerPrefs.SetInt("BGM", 1);
            PlayerPrefs.SetInt("SFX", 1);
            PlayerPrefs.SetInt("LANG", 0);
        }
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("SaveTime", SceneManager.Ins.Scene.SaveTime);
        Debug.Log("Save");
    }

    public void SaveSoundInfo()
    {
        PlayerPrefs.SetInt("BGM", BGM);
        PlayerPrefs.SetInt("SFX", SFX);
        Debug.Log("사운드정보 저장 : " + "BGM " + BGM + ", SFX " + SFX);
    }

    public void SaveLanguage()
    {
        PlayerPrefs.SetInt("LANG", LANG);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("SaveTime"))
        {
            MaxSaveTime = PlayerPrefs.GetFloat("SaveTime");
            BGM = PlayerPrefs.GetInt("BGM");
            SFX = PlayerPrefs.GetInt("SFX");
            LANG = PlayerPrefs.GetInt("LANG");
            Debug.Log("Load");
        }
    }
}
