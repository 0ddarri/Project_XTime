using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] CameraBase[] Cams;
    [Space(5.0f)]
    [SerializeField] CAMERA_TYPE CurrentCamera;
    [Header("TV")]
    [SerializeField] TVBase EnvCheckTV;
    [SerializeField] TVBase PolCheckTV;
    [SerializeField] List<TVBase> TVList = new List<TVBase>();

    public bool IsEnvChecked = false; // È¯°æ ÀßÂïÇû´ÂÁö
    public bool IsPolChecked = false; // ¿À¿° ÀßÂïÇû´ÂÁö

    public void Initialize()
    {
        for(int i = 0; i < TVList.Count; i++)
        {
            TVList[i].Initialize();
        }
    }

    private void Start()
    {
        Initialize();
    }

    CameraBase GetCurrentCamera()
    {
        return FindCamera(CurrentCamera);
    }

    CameraBase FindCamera(CAMERA_TYPE type)
    {
        for (int i = 0; i < Cams.Length; i++)
        {
            if (Cams[i].Type.Equals(type))
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
        if(Input.GetKeyDown(KeyCode.F9))
        {
            UploadNews(CAMERA_TYPE.ENV);
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            UploadNews(CAMERA_TYPE.POL);
        }
        FollowMouse();
    }

    public void UploadNews(CAMERA_TYPE type)
    {
        CameraBase cam = FindCamera(type);
        if (!cam.IsFilmed)
            return;

        cam.IsFilmed = false;

        if (cam.TargetChecked)
        {
            if (type.Equals(CAMERA_TYPE.ENV))
                IsEnvChecked = true;
            else
                IsPolChecked = true;
        }
        else
        {
            if (type.Equals(CAMERA_TYPE.ENV))
                IsEnvChecked = false;
            else
                IsPolChecked = false;
        }

        for(int i = 0; i < TVList.Count; i++)
        {
            TVList[i].SetMaterial(type);
        }
        Debug.Log("ÃÔ¿µ! : " + type);
    }
}
