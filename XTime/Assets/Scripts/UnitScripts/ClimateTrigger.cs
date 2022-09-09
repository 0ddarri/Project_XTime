using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateTrigger : MonoBehaviour
{
    public bool IsClimateTriggered;
    public ENV_TYPE TriggeredClimate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Env"))
        {
            TriggeredClimate = other.GetComponentInParent<BaseClimate>().Type;
            IsClimateTriggered = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsClimateTriggered && collision.tag.Equals("Env"))
        {
            TriggeredClimate = collision.GetComponentInParent<BaseClimate>().Type;
            IsClimateTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Env"))
        {
            TriggeredClimate = ENV_TYPE.NONE;
            IsClimateTriggered = false;
        }
    }
}
