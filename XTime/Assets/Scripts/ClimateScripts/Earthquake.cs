using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : BaseClimate
{
    [SerializeField] ParticleSystem particleEarthquakeMain = null;
    [SerializeField] ParticleSystem particleEarthquakeFinish = null;

    bool isFinishParticle = false;

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
        particleEarthquakeMain.Play();
        particleEarthquakeFinish.Stop();
        yield return StartCoroutine(base.StartClimate(maxTime));
        yield return StartCoroutine(SetGraphicsSettings());
        Debug.Log("지진 시작");

        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            if (!isFinishParticle && CurrentPlayingTime > maxTime - 1.2f)
            {
                isFinishParticle = true;
                particleEarthquakeMain.Stop();
                particleEarthquakeFinish.Play();
            }
            yield return null;
        }

        base.EndClimate();
    }
}