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

    public bool IsEnvChecked = false; // È¯°æ ÀßÂïÇû´ÂÁö
    public bool IsPolChecked = false; // ¿À¿° ÀßÂïÇû´ÂÁö
    [Space(5.0f)]
    [SerializeField] IsoButton[] CamDisableButtons;

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

    public bool IsCameraCompleteFilmed(CAMERA_TYPE type)
    {
        if(type == CAMERA_TYPE.ENV)
        {
            if(IsEnvChecked)
                return true;
            return false;
        }
        else
        {
            if (IsPolChecked)
                return true;
            return false;
        }
    }

    void SetCamDisable()
    {
        for (int i = 0; i < CamDisableButtons.Length; i++)
        {
            if (CamDisableButtons[i].IsEntered)
            {
                GetCurrentCamera().CameraAvail = false;
                return;
            }
        }
        GetCurrentCamera().CameraAvail = true;
    }

    private void Update()
    {
        if (SceneManager.Ins.Scene.IsState(GAME_STATE.INTRO))
            return;

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
            if(IsCameraCompleteFilmed(CAMERA_TYPE.POL))
            {
                SceneManager.Ins.Scene.TrashManager.ClearTrash();
                SceneManager.Ins.Scene.buildingManager.Emotion += 0.1f;
                Debug.Log("½Å°í·Î ÀÎÇÑ ¾Ç°¨Á¤ Áõ°¡");
                IsPolChecked = false;
            }
        }
        FollowMouse();

        SetCamDisable();
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
            {
                IsEnvChecked = true;
                SceneManager.Ins.Scene.SoundManager.PlaySound(SOUND_TYPE.SFX, "EnvUpload");
            }
            else
            {
                IsPolChecked = true;
                SceneManager.Ins.Scene.SoundManager.PlaySound(SOUND_TYPE.SFX, "PolUpload");
            }
            SceneManager.Ins.Scene.MoneyController.Money += 2;
        }
        else
        {
            if (type.Equals(CAMERA_TYPE.ENV))
                IsEnvChecked = false;
            else
                IsPolChecked = false;
            SceneManager.Ins.Scene.MoneyController.Money -= 1;
        }
        TVManager.Upload(type);
        Debug.Log("ÃÔ¿µ! : " + type);
    }
}
