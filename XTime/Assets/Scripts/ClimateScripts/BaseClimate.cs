using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 이상기후 부모클래스
/// </summary>
public class BaseClimate : MonoBehaviour
{
    [Header("Climate Base Settings")]
    public ENV_TYPE Type;
    public bool IsPlaying = false; // 기후 실행중인지 확인
    [SerializeField] protected float CurrentPlayingTime = 0.0f; // 기후 실행시간
    [SerializeField] protected float MinPlayingTime;
    [SerializeField] protected float MaxPlayingTime;
    [SerializeField] public float RandomTime;
    [Space(5.0f)]
    [SerializeField] protected Color TopColor; 
    [SerializeField] protected Color MiddleColor; 
    [SerializeField] protected Color BottomColor;
    [SerializeField] protected Color BottomGradientColor = Color.white;
    public virtual void Initialize() // 초기화 함수
    {
        gameObject.SetActive(false);
        IsPlaying = false;
        CurrentPlayingTime = 0.0f;
    }

    public void StartClimate()
    {
        gameObject.SetActive(true);
        RandomTime = Random.Range(MinPlayingTime, MaxPlayingTime);
        Debug.Log("시작!");
        StartCoroutine(StartClimate(RandomTime));
    }

    protected virtual IEnumerator StartClimate(float maxTime) // 기후 실행하기
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
