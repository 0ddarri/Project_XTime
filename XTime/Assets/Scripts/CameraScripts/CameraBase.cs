using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERA_TYPE
{
    ENV, // 이상기후
    POL, // 오염행위
    NONE // 현재 보도된 종류 없을때
}

public class CameraBase : MonoBehaviour
{
    [Header("Settings")]
    public CAMERA_TYPE Type;
    public Camera Camera;
    [SerializeField] float CameraShotDelay;
    float CurrentCamShotTime = 0.0f;
    [Space(5.0f)]
    [SerializeField] float Width;
    [SerializeField] float Height;
    [Space(5.0f)]
    [SerializeField] float MaxOrthoSize = 10;
    [SerializeField] float MinOrthoSize = 3;
    [SerializeField] float OrthoScalingSpeed = 2;
    [Space(5.0f)]
    [SerializeField] string CollisionTag;

    [Header("Image")]
    [SerializeField] SpriteRenderer LUp;
    [SerializeField] SpriteRenderer LDown;
    [SerializeField] SpriteRenderer RUp;
    [SerializeField] SpriteRenderer RDown;

    [Header("Raycast Settings")]
    [SerializeField] float MaxDistance;

    public bool TargetChecked = false;
    public bool IsFilmed = false;
    bool CamAvail = true;
    public bool CameraAvail
    {
        get
        {
            return CamAvail;
        }
        set
        {
            CamAvail = value;
            if(CamAvail)
            {
                LUp.color = Color.white;
                LDown.color = Color.white;
                RUp.color = Color.white;
                RDown.color = Color.white;
            }
            else
            {
                LUp.color = Color.clear;
                LDown.color = Color.clear;
                RUp.color = Color.clear;
                RDown.color = Color.clear;
            }
        }
    }

    bool IsChecked()
    {
        RaycastHit hit;
        Vector3 size = new Vector3(Width, Height, 5);
        Physics.BoxCast(Camera.transform.position, size / 2, transform.forward, out hit, transform.rotation, MaxDistance);
        if (hit.collider != null && hit.collider.gameObject.tag.Equals(CollisionTag))
        {
            return true;
        }
        return false;
    }

    public IEnumerator CaptureCamera()
    {
        Camera.enabled = true;
        Camera.Render();
        Camera.enabled = false;
        TargetChecked = IsChecked();
        CurrentCamShotTime = 0.0f;
        IsFilmed = true;
        yield return null;
    }

    void UpdateWidthHeight()
    {
        Width = Camera.orthographicSize;
        Height = Width * Screen.height / Screen.width;
    }

    void SetOrthoSize()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Camera.orthographicSize += scroll * Time.deltaTime * OrthoScalingSpeed;
            float ortho = Camera.orthographicSize;
            if (ortho > MaxOrthoSize)
                Camera.orthographicSize = MaxOrthoSize;
            else if (ortho < MinOrthoSize)
                Camera.orthographicSize = MinOrthoSize;
        }
    }

    void SetEdgeSprite()
    {
        Vector2 camPixel = new Vector2(Camera.pixelWidth, Camera.pixelHeight);

        Vector3 camLUpPos = Camera.ScreenToWorldPoint(new Vector3(0, camPixel.y, 0));
        Vector2 LUpSpriteSize = LUp.bounds.size * 0.5f;
        Vector2 LUpPos = new Vector2(camLUpPos.x + LUpSpriteSize.x, camLUpPos.y - LUpSpriteSize.y);
        LUp.gameObject.transform.position = LUpPos;

        Vector3 camRUpPos = Camera.ScreenToWorldPoint(new Vector3(camPixel.x, camPixel.y, 0));
        Vector2 RUpSpriteSize = RUp.bounds.size * 0.5f;
        Vector2 RUpPos = new Vector2(camRUpPos.x - RUpSpriteSize.x, camRUpPos.y - RUpSpriteSize.y);
        RUp.gameObject.transform.position = RUpPos;

        Vector3 camLDownPos = Camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector2 LDownSpriteSize = LDown.bounds.size * 0.5f;
        Vector2 LDownPos = new Vector2(camLDownPos.x + LDownSpriteSize.x, camLDownPos.y + LDownSpriteSize.y);
        LDown.gameObject.transform.position = LDownPos;

        Vector3 camRDownPos = Camera.ScreenToWorldPoint(new Vector3(camPixel.x, 0, 0));
        Vector2 RDownSpriteSize = RDown.bounds.size * 0.5f;
        Vector2 RDownPos = new Vector2(camRDownPos.x - RDownSpriteSize.x, camRDownPos.y + RDownSpriteSize.y);
        RDown.gameObject.transform.position = RDownPos;
    }

    private void Update()
    {
        if(CameraAvail)
        {
            UpdateWidthHeight();
            SetOrthoSize();
            SetEdgeSprite();
            CurrentCamShotTime += Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && CurrentCamShotTime > CameraShotDelay)
            {
                StartCoroutine(CaptureCamera());
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        RaycastHit hit;
        Vector3 size = new Vector3(Camera.orthographicSize, Camera.orthographicSize * Screen.height / Screen.width, 5);
        bool check = Physics.BoxCast(Camera.transform.position, size / 2, transform.forward, out hit, transform.rotation, MaxDistance);
        if(check)
        {
            Gizmos.DrawWireCube(Camera.transform.position + transform.forward * hit.distance, size);
        }
        else
        {
            Gizmos.DrawRay(Camera.transform.position, transform.forward * MaxDistance);
        }
    }
}
