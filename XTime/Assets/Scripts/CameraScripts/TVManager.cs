using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVManager : MonoBehaviour
{
    [SerializeField] List<TVBase> TVBaseList = new List<TVBase>();
    [SerializeField] TVBase EnvCheckTV;
    [SerializeField] TVBase PolCheckTV;
    [Space (5.0f)]
    [SerializeField] float TVUploadStayTime;
    public bool IsTvUploaded = false;
    public CAMERA_TYPE CurrentUploadedType = CAMERA_TYPE.NONE;
    [SerializeField] float CurrentUploadStayTime = 0.0f;

    public bool CurrentUploadComplete = false;

    public void Initialize()
    {
        for(int i = 0; i < TVBaseList.Count; i++)
        {
            TVBaseList[i].Initialize();
        }
        CurrentUploadStayTime = 0.0f;
        IsTvUploaded = false;
        CurrentUploadedType = CAMERA_TYPE.NONE;
    }

    public void Upload(CAMERA_TYPE type)
    {
        CurrentUploadedType = type;
        IsTvUploaded = true;
        CurrentUploadStayTime = 0.0f;
        CameraManager.Ins.CamChangeButton.gameObject.SetActive(false);
        for (int i = 0; i < TVBaseList.Count; i++)
        {
            TVBaseList[i].SetMaterial(type);
        }
    }

    private void Update()
    {
        if(IsTvUploaded)
        {
            CurrentUploadStayTime += Time.deltaTime;
            if(CurrentUploadStayTime > TVUploadStayTime)
            {
                Initialize();
                CameraManager.Ins.CamChangeButton.gameObject.SetActive(true);
            }
        }
    }
}
