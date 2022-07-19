using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public MapManager mapManager;
    public BuildingManager buildingManager;

    private void Awake()
    {
        SceneManager.Ins.Scene = this;
    }
}
