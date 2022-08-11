using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] CameraBase[] Cams;
    [Space(5.0f)]
    [SerializeField] CAMERA_TYPE CurrentCamera;
    [Header("TV")]
    [SerializeField] TVManager TVManager;
    [Header("Buttons")]
    [SerializeField] IsoButton EnvUploadButton;
    [SerializeField] IsoButton PolUploadButton;
    public IsoButton CamChangeButton;

    public bool IsEnvChecked = false; // ȯ�� ����������
    public bool IsPolChecked = false; // ���� ����������

    public void Initialize()
    {
        TVManager.Initialize();
    }

    private void Start()
    {
        Initialize();
    }

    public CameraBase GetCurrentCamera()
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

    void SetCameraAvail()
    {
        if(Input.GetKeyDown(KeyCode.F9))
        {
            GetCurrentCamera().CameraAvail = !GetCurrentCamera().CameraAvail;
            Debug.Log("ī�޶� �״ٰδ�");
        }
    }

    public bool IsCameraCompleteFilmed(CAMERA_TYPE type)
    {
        if(type.Equals(CAMERA_TYPE.ENV))
        {
            if(IsEnvChecked && FindCamera(type).IsFilmed)
                return true;
            return false;
        }
        else
        {
            if (IsPolChecked && FindCamera(type).IsFilmed)
                return true;
            return false;
        }
    }

    private void Update()
    {
        if(CamChangeButton.IsClicked)
        {
            SwapCurrentCamera();
            CamChangeButton.IsClicked = false;
        }
        if(EnvUploadButton.IsClicked)
        {
            UploadNews(CAMERA_TYPE.ENV);
            EnvUploadButton.IsClicked = false;
        }
        if (PolUploadButton.IsClicked)
        {
            UploadNews(CAMERA_TYPE.POL);
            PolUploadButton.IsClicked = false;
        }
        FollowMouse();
        if(EnvUploadButton.IsEntered || PolUploadButton.IsEntered || CamChangeButton.IsEntered)
        {
            GetCurrentCamera().CameraAvail = false;
        }
        else
        {
            GetCurrentCamera().CameraAvail = true;
        }
    }

    public void UploadNews(CAMERA_TYPE type)
    {
        if (TVManager.IsTvUploaded)
            return;

        if(GetCurrentCamera().Type.Equals(type))
        {
            SwapCurrentCamera();
        }

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
        TVManager.Upload(type);
        Debug.Log("�Կ�! : " + type);
    }
}
