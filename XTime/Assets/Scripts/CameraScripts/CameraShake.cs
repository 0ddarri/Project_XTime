using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeAmount = 0.0f;

    float shakeTime = 0.0f;
    Vector3 initialPosition = Vector3.zero;

    void Start()
    {
        initialPosition = transform.position;
    }

    public void StartShake(float time)
    {
        shakeTime = time;
    }

    void Update()
    {
        if (shakeTime > 0.0f)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + initialPosition;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            shakeTime = 0.0f;
            transform.position = initialPosition;
        }
    }
}