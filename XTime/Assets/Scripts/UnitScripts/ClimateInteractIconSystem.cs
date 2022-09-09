using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClimateIcon
{
    public ENV_TYPE Type;
    [SerializeField] SpriteRenderer Sprite;

    public void SetVisible(bool value)
    {
        if (value)
            Sprite.color = Color.white;
        else
            Sprite.color = Color.clear;
    }
}

public class ClimateInteractIconSystem : MonoBehaviour
{
    [SerializeField] List<ClimateIcon> IconList = new List<ClimateIcon>();
    [SerializeField] SpriteRenderer BackgroundSprite;

    public void SetAllInvisible()
    {
        BackgroundSprite.color = Color.clear;
        for(int i = 0; i < IconList.Count; i++)
        {
            IconList[i].SetVisible(false);
        }
    }

    public void SetIcon(ENV_TYPE Type)
    {
        SetAllInvisible();
        BackgroundSprite.color = Color.gray;
        for (int i = 0; i < IconList.Count; i++)
        {
            if (IconList[i].Type.Equals(Type))
            {
                IconList[i].SetVisible(true);
                return;
            }
        }
    }
}
