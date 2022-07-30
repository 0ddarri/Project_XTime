using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateTrigger : MonoBehaviour
{
    public bool IsClimateTriggered;
    public ENV_TYPE TriggeredClimate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Env"))
        {
            TriggeredClimate = other.GetComponentInParent<BaseClimate>().Type;
            IsClimateTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Env"))
        {
            TriggeredClimate = ENV_TYPE.NONE;
            IsClimateTriggered = false;
        }
    }
}
