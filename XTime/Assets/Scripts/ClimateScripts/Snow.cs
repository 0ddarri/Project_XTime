using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : BaseClimate
{
    [SerializeField] Color ColorAdjColor;
    [SerializeField] ParticleSystem particleSnowMain = null;
    [SerializeField] ParticleSystem particleSnowFinish = null;

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
        Color origincolorAdj = SceneManager.Ins.Scene.ClimateManager.ColorAdj.colorFilter.value;

        while (time <= 1.0f)
        {
            time += Time.deltaTime;
            SceneManager.Ins.Scene.ClimateManager.BackgroundMat.SetColor("_TopColor", Color.Lerp(originTopColor, TopColor, time));
            SceneManager.Ins.Scene.ClimateManager.BackgroundMat.SetColor("_MiddleColor", Color.Lerp(originMiddleColor, MiddleColor, time));
            SceneManager.Ins.Scene.ClimateManager.BackgroundMat.SetColor("_BottomColor", Color.Lerp(originBottomColor, BottomColor, time));
            SceneManager.Ins.Scene.ClimateManager.BottomGradientSprite.color = Color.Lerp(originBottomGradientColor, BottomGradientColor, time);
            SceneManager.Ins.Scene.ClimateManager.ColorAdj.colorFilter.value = Color.Lerp(origincolorAdj, ColorAdjColor, time);

            yield return null;
        }
    }

    protected override IEnumerator StartClimate(float maxTime)
    {
        particleSnowMain.Play();
        particleSnowFinish.Stop();
        yield return StartCoroutine(base.StartClimate(maxTime));
        yield return StartCoroutine(SetGraphicsSettings());
        Debug.Log("Æø¼³ ½ÃÀÛ");

        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            if (!isFinishParticle && CurrentPlayingTime > maxTime - 2.0f)
            {
                isFinishParticle = true;
                particleSnowMain.Stop();
                particleSnowFinish.Play();
            }
            yield return null;
        }

        base.EndClimate();
    }
}