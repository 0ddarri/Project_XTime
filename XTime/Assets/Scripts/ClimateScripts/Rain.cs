using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : BaseClimate
{
    [SerializeField] ParticleSystem particleRainMain = null;
    [SerializeField] ParticleSystem particleRainFinish = null;

    bool isFinishParticle = false;

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override IEnumerator StartClimate(float maxTime)
    {
        particleRainMain.Play();
        particleRainFinish.Stop();
        yield return StartCoroutine(base.StartClimate(maxTime));
        Debug.Log("Æø¿ì ½ÃÀÛ");

        CurrentPlayingTime = 0.0f;
        while (CurrentPlayingTime < maxTime)
        {
            CurrentPlayingTime += Time.deltaTime;
            if (!isFinishParticle && CurrentPlayingTime > maxTime - 1.4f)
            {
                isFinishParticle = true;
                particleRainMain.Stop();
                particleRainFinish.Play();
            }
            yield return null;
        }

        isFinishParticle = false;
        base.EndClimate();
    }
}