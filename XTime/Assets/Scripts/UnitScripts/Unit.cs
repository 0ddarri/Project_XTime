using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum UNIT_STATE // �ش� ������ �������� �ȶ�������
{
    NORMAL,
    IN_BUILDING,
    LEAVED
}

public enum MOVEMENT_STATE // ������ ����, ���ĺ��� �� ����Ѵ�
{
    NORMAL
}

public enum MOVE_DIRECTION
{
    LUP,
    LDOWN,
    RUP,
    RDOWN
}

public class MoveDirection
{
    static readonly Vector2[] Direction = new Vector2[]
    {
        new Vector2(-1.0f,0.5f),
        new Vector2(-1.0f,-0.5f),
        new Vector2(1.0f,0.5f),
        new Vector2(1.0f,-0.5f),
    };

    public static Vector2 GetDirection(MOVE_DIRECTION dir)
    {
        return Direction[(int)dir];
    }

    public static Vector2 GetRandomDirection()
    {
        int num = Random.Range(0, Direction.Length);
        return Direction[num];
    }
}

public class Unit : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] Rigidbody2D Rb2D;
    [SerializeField] Collider2D Collider2D;
    [SerializeField] Transform RayTransform;
    [SerializeField] float RayLength;
    [SerializeField] ClimateInteractIconSystem ClimateIcon;
    
    [Header ("State")]
    public float Confidence; // �ŷڵ�
    [SerializeField] UNIT_STATE UnitState;
    [SerializeField] MOVEMENT_STATE MovementState;
    [SerializeField] ClimateTrigger ClimateTrigger;
    [Header ("Status")]
    [SerializeField] float MovementSpeed;
    [SerializeField] float ChangeMoveTimeMin;
    [SerializeField] float ChangeMoveTimeMax;
    [Space(5.0f)]
    [SerializeField] float BuildingStayMinTime = 0.0f;
    [SerializeField] float BuildingStayMaxTime = 0.0f;
    [SerializeField] float BuildingStayTime = 0.0f;
    [SerializeField] float CurrentBuildingStayTime = 0.0f;

    [SerializeField] List<Vector3> nearPos = new List<Vector3>();

    float tileMoveCurTime = 0.0f;
    Vector3 BeforeTilePos;
    Vector3 ResultNearPos;
    [SerializeField] GameObject BeforeTileVisualize;

    Vector3 BuildingOutPosition;

    [Header("Building Info")]
    [SerializeField] List<int> MapIndexList = new List<int>();

    public void Initialize()
    {
        Confidence = 100;
        BeforeTilePos = SceneManager.Ins.Scene.mapManager.GetCellWorldPos(SceneManager.Ins.Scene.mapManager.GetCellPos(transform.position));
        ResultNearPos = BeforeTilePos;

        BuildingStayTime = Random.Range(BuildingStayMinTime, BuildingStayMaxTime);
        CurrentBuildingStayTime = 0.0f;
        MapIndexList = SceneManager.Ins.Scene.buildingManager.GetRandomBuildingNum();

        ClimateIcon.SetAllInvisible();
    }

    private void Start()
    {
        Initialize();
    }

    void StaticTileMove()
    {
        tileMoveCurTime += Time.deltaTime;
        if(tileMoveCurTime > 0.2f)
        {
            tileMoveCurTime = 0.0f;
            Vector3 CheckPos = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
            nearPos = SceneManager.Ins.Scene.mapManager.GetNearMovableWorldCellPos(CheckPos);

            int random = 0;
            if(nearPos.Count != 1)
            {
                random = Random.Range(0, nearPos.Count);
                while (nearPos[random] == BeforeTilePos)
                    random = Random.Range(0, nearPos.Count);
            }

            BeforeTilePos = new Vector3(ResultNearPos.x, ResultNearPos.y -0.25f, ResultNearPos.z);
            ResultNearPos = new Vector3(nearPos[random].x, nearPos[random].y + 0.25f, nearPos[random].z);
            transform.position = ResultNearPos;
        }
    }

    void SetClimateInteract()
    {
        ENV_TYPE type = ClimateTrigger.TriggeredClimate;
        switch (type)
        {
            case ENV_TYPE.NONE:
                {
                    ClimateIcon.SetAllInvisible();
                }
                break;
            case ENV_TYPE.MIST:
                {

                }
                break;
            case ENV_TYPE.YELLOW_DUST:
                {
                    ClimateIcon.SetIcon(ENV_TYPE.YELLOW_DUST);
                }
                break;
        }
    }

    private void Update()
    {
        switch (UnitState)
        {
            case UNIT_STATE.NORMAL:
                {
                    StaticTileMove();
                    SetClimateInteract();
                    BeforeTileVisualize.transform.position = BeforeTilePos;
                }
                break;
            case UNIT_STATE.IN_BUILDING:
                {
                    CurrentBuildingStayTime += Time.deltaTime;
                    if(CurrentBuildingStayTime > BuildingStayTime)
                    {
                        CurrentBuildingStayTime = 0.0f;
                        ChangeState(UNIT_STATE.NORMAL);
                    }
                }
                break;
        }
    }

    void StateInit_Normal()
    {
        Vector3Int cellpos = SceneManager.Ins.Scene.mapManager.GetCellPos(BuildingOutPosition);
        transform.position = SceneManager.Ins.Scene.mapManager.GetCellWorldPos(cellpos);
        Invoke("INV_ColliderOn", 1.0f);
    }

    void INV_ColliderOn()
    {
        Collider2D.enabled = true;
    }

    void StateInit_InBuilding()
    {
        transform.position = new Vector3(-100000, 0, 0);
        Collider2D.enabled = false;
    }

    void StateInit_Leave()
    {

    }

    void ChangeState(UNIT_STATE newstate)
    {
        if (UnitState.Equals(newstate))
            return;

        switch(newstate)
        {
            case UNIT_STATE.NORMAL:
                {
                    StateInit_Normal();
                }
                break;
            case UNIT_STATE.IN_BUILDING:
                {
                    StateInit_InBuilding();
                }
                break;
            case UNIT_STATE.LEAVED:
                {
                    StateInit_Leave();
                }
                break;
        }

        UnitState = newstate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enterance")
        {
            Entrance enter = collision.GetComponent<Entrance>();
            BaseBuilding building = enter.GetComponentInParent<BaseBuilding>();
            bool noChecked = true;
            for(int i = 0; i < MapIndexList.Count; i++)
            {
                if (building.Index.Equals(MapIndexList[i]))
                {
                    noChecked = false;
                    continue;
                }
            }
            if (noChecked)
                return;

            enter.IsEnter = true;
            BuildingOutPosition = enter.GetComponentInParent<BaseBuilding>().GetRandomEntrance().transform.position;
            ChangeState(UNIT_STATE.IN_BUILDING);
        }
    }
}
