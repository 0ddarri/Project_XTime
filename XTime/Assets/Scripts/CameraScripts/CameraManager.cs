using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] CameraBase[] Cams;
    [Space(5.0f)]
    [SerializeField] CAMERA_TYPE CurrentCamera;

    CameraBase GetCurrentCamera()
    {
        for (int i = 0; i < Cams.Length; i++)
        {
            if(Cams[i].Type.Equals(CurrentCamera))
                return Cams[i];
        }
        Debug.Log("ERROR!: Can't Find Camera");
        return null;
    }

    void SwapCurrentCamera()
    {
        GetCurrentCamera().gameObject.SetActive(false);
        CurrentCamera = 1 - CurrentCamera;
        GetCurrentCamera().gameObject.SetActive(true);
    }

    void FollowMouse()
    {
        GetCurrentCamera().transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F10))
            SwapCurrentCamera();
        FollowMouse();
    }

    public void test()
    {
        print("asdfasdfasdfasdfasdfasdf");
    }
}
