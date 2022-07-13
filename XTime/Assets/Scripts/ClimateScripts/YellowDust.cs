using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowDust : BaseClimate
{
    [Header ("Dust Settings")]
    [SerializeField] GameObject Dust;
    [SerializeField] Transform DustLeftPos;
    [SerializeField] Transform DustRightPos;

    public override void Initialize()
    {
        base.Initialize();
        StartCoroutine(StartClimate(Random.Range(MinPlayingTime, MaxPlayingTime)));
    }

    protected override IEnumerator StartClimate(float maxTime)
    {
        yield return StartCoroutine(base.StartClimate(maxTime));
        Debug.Log("황사 시작");

        bool isLeft = Random.Range(0, 2) == 1 ? true : false;
        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            float LerpTime = CurrentPlayingTime / maxTime;
            if(isLeft)
                Dust.transform.position = Vector3.Lerp(DustLeftPos.position, DustRightPos.position, LerpTime);
            else
                Dust.transform.position = Vector3.Lerp(DustLeftPos.position, DustRightPos.position, LerpTime);

            yield return null;
        }

        IsPlaying = false;
    }

    private void Start()
    {
    }
}
