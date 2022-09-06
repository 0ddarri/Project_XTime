using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDestroy : MonoBehaviour
{
    [SerializeField] ParticleSystem particleMain = null;
    [SerializeField] SpriteRenderer[] rendererArray = { };

    float duration = 1.0f;

    void Start()
    {
        rendererArray[0].material.SetFloat("_Fade", duration);
        rendererArray[1].material.SetFloat("_Fade", duration);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            particleMain.Play();

        if (particleMain == null)
        {
            duration -= Time.deltaTime;
            rendererArray[0].material.SetFloat("_Fade", duration);
            rendererArray[1].material.SetFloat("_Fade", duration);
            if (duration <= 0.0f)
                Destroy(gameObject);
        }
    }
}