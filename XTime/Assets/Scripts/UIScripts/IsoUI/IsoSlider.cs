using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsoSlider : MonoBehaviour
{
    [SerializeField] RectTransform FillImage;

    public void Initialize()
    {
        FillImage.localScale = Vector3.one;
    }

    public void SetFillImage(float start, float end, float value)
    {
        float t = (start - value) / (start - end);
        Vector3 scale = FillImage.localScale;
        FillImage.localScale = new Vector3(t, scale.y, scale.z);
    }
}
