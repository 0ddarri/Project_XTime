using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Hindrance : MonoBehaviour
{
    [SerializeField] Volume volume = null;
    [SerializeField] float hindranceDelayMin = 0.0f;
    [SerializeField] float hindranceDelayMax = 0.0f;
    [SerializeField] float hindranceCoolMin = 0.0f;
    [SerializeField] float hindranceCoolMax = 0.0f;

    bool onHindrance = false;
    Vignette vignette = null;

    IEnumerator Enum_BaseHindrance(float _intensityValue)
    {
        float time = 0.0f;
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            float originValue = vignette.intensity.value;
            while (time <= 1.0f)
            {
                time += Time.deltaTime;
                vignette.intensity.value = Mathf.Lerp(originValue, _intensityValue, time);
                yield return null;
            }
        }
    }

    IEnumerator Enum_StartHindrance()
    {
        onHindrance = true;
        yield return StartCoroutine(Enum_BaseHindrance(0.75f));
        float delay = Random.Range(hindranceDelayMin, hindranceDelayMax);
        StartCoroutine(Enum_Hindrance(delay));
    }

    IEnumerator Enum_Hindrance(float _delay)
    {
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            float time = 0.0f;
            float originValue = vignette.intensity.value;
            bool isMin = false;
            while (_delay >= 0.0f)
            {
                _delay -= Time.deltaTime;
                if (isMin)
                    time -= Time.deltaTime;
                else
                    time += Time.deltaTime;
                vignette.intensity.value = Mathf.Lerp(originValue, 0.6f, time);

                if (vignette.intensity.value <= 0.6f)
                    isMin = true;
                else if (vignette.intensity.value >= 0.75f)
                    isMin = false;

                yield return null;
            }
        }

        yield return StartCoroutine(Enum_BaseHindrance(0.0f));
        float cool = Random.Range(hindranceCoolMin, hindranceCoolMax);
        yield return new WaitForSeconds(cool);
        onHindrance = false;
    }

    void Update()
    {
        if (!onHindrance && SceneManager.Ins.Scene.buildingManager.Emotion >= 0.1f)
            StartCoroutine(Enum_StartHindrance());
    }
}