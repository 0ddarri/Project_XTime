using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// �̻���� �θ�Ŭ����
/// </summary>
public class BaseClimate : MonoBehaviour
{
    [Header("Climate Base Settings")]
    public ENV_TYPE Type;
    public bool IsPlaying = false; // ���� ���������� Ȯ��
    [SerializeField] protected float CurrentPlayingTime = 0.0f; // ���� ����ð�
    [SerializeField] protected float MinPlayingTime;
    [SerializeField] protected float MaxPlayingTime;
    [SerializeField] public float RandomTime;
    [Space(5.0f)]
    [SerializeField] protected Color TopColor; 
    [SerializeField] protected Color MiddleColor; 
    [SerializeField] protected Color BottomColor;
    [SerializeField] protected Color BottomGradientColor = Color.white;
    public virtual void Initialize() // �ʱ�ȭ �Լ�
    {
        gameObject.SetActive(false);
        IsPlaying = false;
        CurrentPlayingTime = 0.0f;
    }

    public void StartClimate()
    {
        gameObject.SetActive(true);
        RandomTime = Random.Range(MinPlayingTime, MaxPlayingTime);
        Debug.Log("����!");
        StartCoroutine(StartClimate(RandomTime));
    }

    protected virtual IEnumerator StartClimate(float maxTime) // ���� �����ϱ�
    {
        IsPlaying = true;
        yield return null;
    }

    protected void EndClimate()
    {
        IsPlaying = false;
        gameObject.SetActive(false);
    }

    public BaseClimate GetComp()
    {
        return this;
    }
}
