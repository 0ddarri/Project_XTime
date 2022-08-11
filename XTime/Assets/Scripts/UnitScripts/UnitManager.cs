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

    [SerializeField] Text TestConfidenceText;
    [SerializeField] Text TestPopulationText;

    private void Start()
    {
        Population = UnitList.Count;
    }

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
        Debug.Log("��δ��� : " + result);

        result /= UnitList.Count;
        Debug.Log("���� : " + result);

        ConfidenceLevel = result;
        Debug.Log(result + "%");
        TestConfidenceText.text = "�ŷڵ� : " + result + "%";
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
        TestPopulationText.text = "���� �α����� : " + PopulationLevel + "%";
    }

    void DiscountAllConfidence(int min, int max)
    {
        for (int i = 0; i < UnitList.Count; i++)
        {
            UnitList[i].Confidence -= Random.Range(min, max);
        }
        Debug.Log("�ŷڵ� ����");
    }

    private void Update()
    {
        CalcConfidenceAverage();
        CalcPopulation();
    }

}
