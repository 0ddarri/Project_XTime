using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : BaseClimate
{
    [SerializeField] ParticleSystem particleSnowMain = null;
    [SerializeField] ParticleSystem particleSnowFinish = null;

    bool isFinishParticle = false;

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override IEnumerator StartClimate(float maxTime)
    {
        particleSnowMain.Play();
        particleSnowFinish.Stop();
        yield return StartCoroutine(base.StartClimate(maxTime));
        Debug.Log("Æø¼³ ½ÃÀÛ");

        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            if (!isFinishParticle && CurrentPlayingTime > maxTime - 2.0f)
            {
                isFinishParticle = true;
                particleSnowMain.Stop();
                particleSnowFinish.Play();
            }
            yield return null;
        }

        base.EndClimate();
    }
}