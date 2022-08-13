using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoNumber : MonoBehaviour
{
    [SerializeField] List<Sprite> NumberTextureList = new List<Sprite>();

    public Sprite GetCurrentHourSprite()
    {
        int curHour = System.DateTime.Now.Hour;
        if (curHour > 12)
            curHour -= 12;
        return NumberTextureList[curHour];
    }
}
