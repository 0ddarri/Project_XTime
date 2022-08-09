using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVBase : MonoBehaviour
{
    [SerializeField] MeshRenderer Mesh;

    [SerializeField] Material DefaultMaterial;
    [SerializeField] Material CameraEnvMaterial;
    [SerializeField] Material CameraPolMaterial;

    public void Initialize()
    {
        Mesh.material = DefaultMaterial;
    }

    public void SetMaterial(CAMERA_TYPE type)
    {
        if (type.Equals(CAMERA_TYPE.ENV))
            Mesh.material = CameraEnvMaterial;
        else
            Mesh.material = CameraPolMaterial;
    }
}
