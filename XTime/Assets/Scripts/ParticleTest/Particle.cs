using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleArray = { };

    bool Alive()
    {
        for (int i = 0; i < particleArray.Length; i++)
        {
            if (particleArray[i] != null)
                return true;
        }

        return false;
    }

    void Update()
    {
        if (!Alive())
            Destroy(gameObject);
    }
}