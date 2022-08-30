using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] float ConfidenceLevel;
    [SerializeField] int Population;
    [SerializeField] float PopulationLevel;

    [SerializeField] List<Unit> UnitList = new List<Unit>();

    [SerializeField] Transform ConfidenceFill;

    private void Start()
    {
        Population = UnitList.Count;

        SetUnitAlpha(0);
    }

    public void SetUnitAlpha(float value)
    {
        for (int i = 0; i < UnitList.Count; i++)
        {
            UnitList[i].SetAlpha(value);
        }
    }

    public float Confidence
    {
        get
        {
            return ConfidenceLevel;
        }
    }

    void SetConFidenceFill(float value)
    {
        Vector3 scale = ConfidenceFill.lossyScale;
        ConfidenceFill.localScale = new Vector3(value, scale.y, scale.z);
    }

    void CalcConfidenceAverage()
    {
        float percent = 0.0f;

        for(int i = 0; i < UnitList.Count; i++)
        {
            percent += UnitList[i].Confidence;
        }
        Debug.Log("¸ðµÎ´õÇÔ : " + percent);

        float val = percent / UnitList.Count;
        Debug.Log("³ª´® : " + val);
        SetConFidenceFill(val * 0.01f);

        ConfidenceLevel = val;
    }

    int CheckAvailUnit()
    {
        int count = 0;
        for(int i = 0; i < UnitList.Count; i++)
        {
            if(!UnitList[i].UnitState.Equals(UNIT_STATE.LEAVED))
                count++;
        }
        return count;
    }

    void CalcPopulation()
    {
        PopulationLevel = CheckAvailUnit() / Population * 100;
        Debug.Log(PopulationLevel);
    }

    void DiscountAllConfidence(int min, int max)
    {
        for (int i = 0; i < UnitList.Count; i++)
        {
            UnitList[i].Confidence -= Random.Range(min, max);
        }
        Debug.Log("½Å·Úµµ ±ðÀÓ");
    }

    private void Update()
    {
        CalcConfidenceAverage();
        CalcPopulation();
    }

}
