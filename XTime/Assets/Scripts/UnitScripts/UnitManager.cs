using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] float ConfidenceLevel;

    [SerializeField] List<Unit> UnitList = new List<Unit>();

    public float Confidence
    {
        get
        {
            return ConfidenceLevel;
        }
    }

    void CalcConfidenceAverage()
    {
        float result = 0.0f;

        for(int i = 0; i < UnitList.Count; i++)
        {
            result += UnitList[i].Confidence;
        }

        result /= UnitList.Count;

        ConfidenceLevel = result;
        Debug.Log(result + "%");
    }

    private void Update()
    {
        CalcConfidenceAverage();
    }

}