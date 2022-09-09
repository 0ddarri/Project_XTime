using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : BaseClimate
{
    [Header ("Dust Settings")]
    [SerializeField] GameObject Dust;
    [SerializeField] Transform DustLeftPos;
    [SerializeField] Transform DustRightPos;

    public override void Initialize()
    {
        base.Initialize();
    }

    IEnumerator SetGraphicsSettings()
    {
        float time = 0.0f;

        Color originTopColor = SceneManager.Ins.Scene.ClimateManager.BackgroundMat.GetColor("_TopColor");
        Color originMiddleColor = SceneManager.Ins.Scene.ClimateManager.BackgroundMat.GetColor("_MiddleColor");
        Color originBottomColor = SceneManager.Ins.Scene.ClimateManager.BackgroundMat.GetColor("_BottomColor");
        Color originBottomGradientColor = SceneManager.Ins.Scene.ClimateManager.BottomGradientSprite.color;

        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            SceneManager.Ins.Scene.ClimateManager.BackgroundMat.SetColor("_TopColor", Color.Lerp(originTopColor, TopColor, time));
            SceneManager.Ins.Scene.ClimateManager.BackgroundMat.SetColor("_MiddleColor", Color.Lerp(originMiddleColor, MiddleColor, time));
            SceneManager.Ins.Scene.ClimateManager.BackgroundMat.SetColor("_BottomColor", Color.Lerp(originBottomColor, BottomColor, time));
            SceneManager.Ins.Scene.ClimateManager.BottomGradientSprite.color = Color.Lerp(originBottomGradientColor, BottomGradientColor, time);

            yield return null;
        }
    }

    protected override IEnumerator StartClimate(float maxTime)
    {
        yield return StartCoroutine(base.StartClimate(maxTime));
        yield return StartCoroutine(SetGraphicsSettings());
        Debug.Log("황사 시작");

        bool isLeft = Random.Range(0, 2) == 1 ? true : false;
        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            float LerpTime = CurrentPlayingTime / maxTime;
            if(isLeft)
                Dust.transform.position = Vector3.Lerp(DustLeftPos.position, DustRightPos.position, LerpTime);
            else
                Dust.transform.position = Vector3.Lerp(DustLeftPos.position, DustRightPos.position, LerpTime);

            yield return null;
        }

        base.EndClimate();
    }
}
