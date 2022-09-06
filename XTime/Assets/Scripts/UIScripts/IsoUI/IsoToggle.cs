using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoToggle : MonoBehaviour
{
    IsoButton Button;
    public bool Toggle;

    // Start is called before the first frame update
    void Start()
    {
        Button = GetComponentInChildren<IsoButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Button.IsClicked)
        {
            Button.IsClicked = false;
            Toggle = !Toggle;
        }
    }
}
