using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTitle : MonoBehaviour
{
    [SerializeField] SpriteRenderer TimeSprite;
    [SerializeField] IsoNumber Number;

    void SetTimeSprite()
    {
        TimeSprite.sprite = Number.GetCurrentHourSprite();
    }

    private void Update()
    {
        SetTimeSprite();
    }
}
