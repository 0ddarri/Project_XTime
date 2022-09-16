using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSync : MonoBehaviour
{
    [SerializeField] SpriteRenderer SyncedSprite;
    SpriteRenderer SpriteRenderer;
    [Space(10.0f)]
    [SerializeField] bool ColorSync = false;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(ColorSync)
        {
            SpriteRenderer.color = SyncedSprite.color;
        }
    }
}