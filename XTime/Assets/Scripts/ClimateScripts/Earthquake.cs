using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : BaseClimate
{
    [SerializeField] ParticleSystem particleEarthquakeMain = null;
    [SerializeField] ParticleSystem particleEarthquakeFinish = null;

    bool isFinishParticle = false;

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override IEnumerator StartClimate(float maxTime)
    {
        particleEarthquakeMain.Play();
        particleEarthquakeFinish.Stop();
        yield return StartCoroutine(base.StartClimate(maxTime));
        Debug.Log("지진 시작");

        CurrentPlayingTime = 0.0f;
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