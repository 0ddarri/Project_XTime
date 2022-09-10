using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : BaseClimate
{
    [SerializeField] List<BaseBuilding> TargetBuildingList = new List<BaseBuilding>();
    [SerializeField] int BuildingTargetMax;

    [SerializeField] ParticleSystem particleEarthquakeMain = null;
    [SerializeField] ParticleSystem particleEarthquakeFinish = null;
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

    IEnumerator BreakBuildings(float maxTime)
    {
        int count = TargetBuildingList.Count;
        float max = maxTime;
        for(int i = 0; i < count; i++)
        {
            float time = Random.Range(0, max);
            yield return new WaitForSeconds(time);
            TargetBuildingList[i].Broken = true;
            max -= time;
            CurrentPlayingTime += time;
            if (!isFinishParticle && CurrentPlayingTime > maxTime - 1.2f)
            {
                isFinishParticle = true;
                particleEarthquakeMain.Stop();
                particleEarthquakeFinish.Play();
            }
        }
    }

    protected override IEnumerator StartClimate(float maxTime)
    {
        Camera.main.GetComponent<CameraShake>().StartShake(maxTime);
        particleEarthquakeMain.Play();
        particleEarthquakeFinish.Stop();

        TargetBuildingList = SceneManager.Ins.Scene.buildingManager.GetRandomBuilding(Random.Range(1, BuildingTargetMax));

        CurrentPlayingTime = 0.0f;
        yield return StartCoroutine(base.StartClimate(maxTime));
        yield return StartCoroutine(SetGraphicsSettings());
        yield return StartCoroutine(BreakBuildings(maxTime));
        Debug.Log("지진 시작");

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