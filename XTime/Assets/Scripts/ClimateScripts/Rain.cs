using BansheeGz.BGSpline.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class Rain : BaseClimate
{
    [SerializeField] Color ColorAdjColor;
    [SerializeField] ParticleSystem particleRainMain = null;
    [SerializeField] ParticleSystem particleRainFinish = null;
    [Space(5.0f)]
    [SerializeField] BoxCollider Collider3D;
    [SerializeField] BoxCollider2D Collider2D;
    [SerializeField] float ColEnableDelay = 0.2f;
     
    bool isFinishParticle = false;

    public override void Initialize()
    {
        base.Initialize();
    }

    IEnumerator IV_ColActive(float delay)
    {
        yield return new WaitForSeconds(delay);
        Collider2D.enabled = true;
    }


    IEnumerator SetGraphicsSettings()
    {
        StartCoroutine(IV_ColActive(ColEnableDelay));

        float time = 0.0f;

        Color originTopColor = SceneManager.Ins.Scene.ClimateManager.BackgroundMat.GetColor("_TopColor");
        Color originMiddleColor = SceneManager.Ins.Scene.ClimateManager.BackgroundMat.GetColor("_MiddleColor");
        Color originBottomColor = SceneManager.Ins.Scene.ClimateManager.BackgroundMat.GetColor("_BottomColor");
        Color originBottomGradientColor = SceneManager.Ins.Scene.ClimateManager.BottomGradientSprite.color;
        Color origincolorAdj = SceneManager.Ins.Scene.ClimateManager.ColorAdj.colorFilter.value;
        Collider2D.enabled = false;

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
        particleRainMain.Play();
        particleRainFinish.Stop();
        yield return StartCoroutine(base.StartClimate(maxTime));
        yield return StartCoroutine(SetGraphicsSettings());
        Debug.Log("Æø¿ì ½ÃÀÛ");

        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            if (!isFinishParticle && CurrentPlayingTime > maxTime - 1.4f)
            {
                isFinishParticle = true;
                particleRainMain.Stop();
                particleRainFinish.Play();
            }
            yield return null;
        }

        isFinishParticle = false;
        base.EndClimate();
    }
}