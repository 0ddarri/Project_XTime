using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : BaseClimate
{
    [SerializeField] List<BaseBuilding> TargetBuildingList = new List<BaseBuilding>();
    [SerializeField] int BuildingTargetMax;
    [SerializeField] GameObject Effect;
    [Header("Spline Settings")]
    [SerializeField] BansheeGz.BGSpline.Curve.BGCurve Curve;

    public override void Initialize()
    {
        base.Initialize();
    }

    bool Vector3Equals(Vector3 one, Vector3 two)
    {
        if (Mathf.Approximately(one.x, two.x) && Mathf.Approximately(one.y, two.y) && Mathf.Approximately(one.z, two.z))
            return true;
        return false;
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
        Debug.Log("토네이도 시작");
        Curve.Clear();
        TargetBuildingList = SceneManager.Ins.Scene.buildingManager.GetRandomBuilding(Random.Range(0, BuildingTargetMax));
        BansheeGz.BGSpline.Curve.BGCurvePoint originP = new BansheeGz.BGSpline.Curve.BGCurvePoint(Curve, Effect.transform.position, 
            BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum.BezierSymmetrical, Vector3.left, Vector3.right);
        Curve.AddPoint(originP);
        for (int i = 0; i < TargetBuildingList.Count; i++)
        {
            BansheeGz.BGSpline.Curve.BGCurvePoint point = new BansheeGz.BGSpline.Curve.BGCurvePoint(Curve, TargetBuildingList[i].transform.position, 
                BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum.BezierSymmetrical, Vector3.left, Vector3.right);
            Curve.AddPoint(point);
        }

        for(int i = 1; i < Curve.PointsCount - 1; i++)
        {
            Vector3 prev = Curve.Points[i - 1].PositionLocal;
            Vector3 next = Curve.Points[i + 1].PositionLocal;
            Vector3 norm = Vector3.Normalize(next - prev);
            Curve.Points[i] = new BansheeGz.BGSpline.Curve.BGCurvePoint(Curve, Curve.Points[i].PositionLocal,
            BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum.BezierSymmetrical, -norm, norm);
        }

        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            yield return null;
        }

        base.EndClimate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Buildings")
        {
            for (int i = 0; i < TargetBuildingList.Count; i++)
            {
                if (TargetBuildingList[i].gameObject.name == collision.gameObject.name)
                {
                    if (!TargetBuildingList[i].Broken)
                        TargetBuildingList[i].Broken = true;
                }
            }
        }
    }
}
