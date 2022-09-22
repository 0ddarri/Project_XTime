using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVBase : MonoBehaviour
{
    [SerializeField] MeshRenderer Mesh;

    [SerializeField] Material DefaultMaterial;
    [SerializeField] Material CameraEnvMaterial;
    [SerializeField] Material CameraPolMaterial;
    [SerializeField] ParticleSystem particleMain = null; 

    public void Initialize()
    {
        Mesh.material = DefaultMaterial;
    }

    public void SetMaterial(CAMERA_TYPE type)
    {
        particleMain.Play();
        if (type.Equals(CAMERA_TYPE.ENV))
            Mesh.material = CameraEnvMaterial;
        else
            Mesh.material = CameraPolMaterial;
    }
}
