using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoButton : MonoBehaviour
{
    public bool IsClicked = false;
    public bool IsEntered = false;

    private void OnMouseDown()
    {
        SceneManager.Ins.Scene.SoundManager.PlaySound(SOUND_TYPE.SFX, "ButtonClick");
        IsClicked = true;
    }

    private void OnMouseUp()
    {
        IsClicked = false;
    }

    private void OnMouseEnter()
    {
        IsEntered = true;
    }

    private void OnMouseExit()
    {
        IsEntered = false;
    }
}
