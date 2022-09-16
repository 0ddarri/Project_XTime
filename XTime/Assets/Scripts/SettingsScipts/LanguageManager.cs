using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LangSprite
{
    public SpriteRenderer Sprite;
    public Sprite KORSprite;
    public Sprite ENGSprite;
}

[System.Serializable]
public class LangString
{
    public LanguageString String;
    public string KORString;
    public string ENGString;
}

public class LanguageManager : MonoBehaviour
{
    public List<LangSprite> LangApplySpriteList = new List<LangSprite>();
    public List<LangString> LangApplyStringList = new List<LangString>();

    public void SetKorean()
    {
        for(int i = 0; i < LangApplySpriteList.Count; i++)
        {
            LangApplySpriteList[i].Sprite.sprite = LangApplySpriteList[i].KORSprite;
        }

        for (int i = 0; i < LangApplyStringList.Count; i++)
        {
            LangApplyStringList[i].String.str = LangApplyStringList[i].KORString;
        }
    }

    public void SetEnglish()
    {
        for (int i = 0; i < LangApplySpriteList.Count; i++)
        {
            LangApplySpriteList[i].Sprite.sprite = LangApplySpriteList[i].ENGSprite;
        }

        for (int i = 0; i < LangApplyStringList.Count; i++)
        {
            LangApplyStringList[i].String.str = LangApplyStringList[i].ENGString;
        }
    }
}
