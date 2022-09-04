using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    protected override IEnumerator StartClimate(float maxTime)
    {
        yield return StartCoroutine(base.StartClimate(maxTime));
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
            Debug.Log("야 수정되고있나");
            Vector3 prev = Curve.Points[i - 1].PositionLocal;
            Vector3 next = Curve.Points[i + 1].PositionLocal;
            Vector3 norm = Vector3.Normalize(next - prev);
            Debug.Log("수정할 벡터 : " + norm);
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
