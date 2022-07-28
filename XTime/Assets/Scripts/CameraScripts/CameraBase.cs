using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERA_TYPE
{
    ENV, // 이상기후
    POL // 오염행위
}

public class CameraBase : MonoBehaviour
{
    public CAMERA_TYPE Type;
    public Camera Camera;
    [Space(5.0f)]
    [SerializeField] float Width;
    [SerializeField] float Height;
    [Space(5.0f)]
    [SerializeField] float MaxOrthoSize = 10;
    [SerializeField] float MinOrthoSize = 3;
    [SerializeField] float OrthoScalingSpeed = 2;

    public IEnumerator CaptureCamera()
    {
        yield return null;

        Camera.enabled = true;
        Camera.Render();
        Camera.enabled = false;
    }

    void UpdateWidthHeight()
    {
        Width = Camera.orthographicSize;
        Height = Width * Screen.width / Screen.height;
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

    }

    private void Update()
    {
        UpdateWidthHeight();
        SetOrthoSize();
        if(Input.GetKeyDown(KeyCode.F11))
        {
            StartCoroutine(CaptureCamera());
        }
    }

}
