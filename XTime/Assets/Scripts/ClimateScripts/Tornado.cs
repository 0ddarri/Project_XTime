using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : BaseClimate
{
    public override void Initialize()
    {
        base.Initialize();
    }

    protected override IEnumerator StartClimate(float maxTime)
    {
        yield return StartCoroutine(base.StartClimate(maxTime));
        Debug.Log("토네이도 시작");

        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            yield return null;
        }

        base.EndClimate();
    }
}
