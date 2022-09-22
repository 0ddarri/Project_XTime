using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UNIT_STATE // 해당 유닛이 떠났는지 안떠났는지
{
    NORMAL,
    IN_BUILDING,
    LEAVED
}

public enum MOVEMENT_STATE // 움직임 상태, 기후별로 다 써야한다
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
    [SerializeField] float CheckOffset = 0.25f;
    
    [Header ("State")]
    [SerializeField] float ConfidenceCount; // 신뢰도
    public UNIT_STATE UnitState;
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

    [SerializeField] int TileNum;

    [Space(5.0f)]
    public bool IsSetConFidenceByEnv = false;
    GameObject CurrentEnv = null;

    [SerializeField] bool IsPolluting = false;

    float tileMoveCurTime = 0.0f;
    Vector3 BeforeTilePos;
    Vector3 ResultNearPos;
    [SerializeField] GameObject BeforeTileVisualize;

    Vector3 BuildingOutPosition;

    [Header("Building Info")]
    [SerializeField] List<int> MapIndexList = new List<int>();


    [Header("Sprites")]
    [SerializeField] SpriteRenderer Fill;
    [SerializeField] SpriteRenderer Outline;

    [Space(0.5f)]
    [SerializeField] SpriteRenderer spriteMainRenderer = null;
    [SerializeField] SpriteRenderer outLineRenderer = null;
    [SerializeField] ParticleSystem particleMain = null;

    float destroyDuration = 1.0f;

    public float Confidence
    {
        get
        {
            return ConfidenceCount;
        }
        set
        {
            ConfidenceCount = value;
            if (ConfidenceCount > 100)
                ConfidenceCount = 100;
            if(ConfidenceCount < 0)
                ConfidenceCount = 0;
        }
    }

    bool isLeave = false;

    public void Initialize()
    {
        isLeave = false;
        destroyDuration = 1.0f;
        spriteMainRenderer.material.SetFloat("_Fade", destroyDuration);
        outLineRenderer.material.SetFloat("_Fade", destroyDuration);
        Confidence = 100;
        BeforeTilePos = SceneManager.Ins.Scene.MapManager.GetCellWorldPos(SceneManager.Ins.Scene.MapManager.GetCellPos(transform.position, TileNum), TileNum);
        ResultNearPos = BeforeTilePos;

        BuildingStayTime = Random.Range(BuildingStayMinTime, BuildingStayMaxTime);
        CurrentBuildingStayTime = 0.0f;
        //MapIndexList = SceneManager.Ins.Scene.buildingManager.GetRandomBuildingNum();

        ClimateIcon.SetAllInvisible();
    }

    public void SetAlpha(float value)
    {
        Color color = Fill.color;
        Fill.color = new Color(color.r, color.g, color.b, value);
        Color outlineColor = Outline.color;
        Outline.color = new Color(outlineColor.r, outlineColor.g, outlineColor.b, value);
    }

    private void Start()
    {
        spriteMainRenderer.material = Instantiate(spriteMainRenderer.material);
        outLineRenderer.material = Instantiate(outLineRenderer.material);
        Initialize();
    }

    void StaticTileMove()
    {
        tileMoveCurTime += Time.deltaTime * 2;
        if(tileMoveCurTime > 1.0f)
        {
            tileMoveCurTime = 0.0f;
            Vector3 CheckPos = new Vector3(ResultNearPos.x, ResultNearPos.y - CheckOffset, ResultNearPos.z);
            nearPos = SceneManager.Ins.Scene.MapManager.GetNearMovableWorldCellPos(CheckPos, TileNum);

            int random = 0;
            if(nearPos.Count != 1)
            {
                random = Random.Range(0, nearPos.Count);
                while (nearPos[random] == BeforeTilePos)
                    random = Random.Range(0, nearPos.Count);
            }

            BeforeTilePos = new Vector3(ResultNearPos.x, ResultNearPos.y - CheckOffset, ResultNearPos.z);
            ResultNearPos = new Vector3(nearPos[random].x, nearPos[random].y + CheckOffset, nearPos[random].z);
        }
        Vector3 RBeforePos = new Vector3(BeforeTilePos.x, BeforeTilePos.y + CheckOffset, BeforeTilePos.z);
        transform.position = Vector3.Lerp(RBeforePos, ResultNearPos, tileMoveCurTime);
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
                    ClimateIcon.SetIcon(ENV_TYPE.MIST);
                }
                break;
            case ENV_TYPE.YELLOW_DUST:
                {
                    ClimateIcon.SetIcon(ENV_TYPE.YELLOW_DUST);
                }
                break;
            case ENV_TYPE.TORNADO:
                {
                    ClimateIcon.SetIcon(ENV_TYPE.TORNADO);
                }
                break;
            case ENV_TYPE.SNOW:
                {
                    ClimateIcon.SetIcon(ENV_TYPE.SNOW);
                }
                break;
            case ENV_TYPE.RAIN:
                {
                    ClimateIcon.SetIcon(ENV_TYPE.RAIN);
                }
                break;
            case ENV_TYPE.QUAKE:
                {
                    ClimateIcon.SetIcon(ENV_TYPE.QUAKE);
                }
                break;
        }
    }


    void CheckLeave()
    {
        if(!isLeave && Confidence < 40)
        {
            isLeave = true;
            ChangeState(UNIT_STATE.LEAVED);
        }
    }
    
    void SetEnvInteract()
    {
        if (!IsSetConFidenceByEnv)
            return;

        if (!CurrentEnv.activeSelf)
            IsSetConFidenceByEnv = false;
    }

    private void Update()
    {
        if (SceneManager.Ins.Scene.IsState(GAME_STATE.INTRO))
            return;

        switch (UnitState)
        {
            case UNIT_STATE.NORMAL:
                {
                    StaticTileMove();
                    SetClimateInteract();
                    BeforeTileVisualize.transform.position = BeforeTilePos;
                    CheckLeave();
                    SetEnvInteract();
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
            case UNIT_STATE.LEAVED:
                {
                    UintDestroy();
                }
                break;
        }
    }

    void StateInit_Normal()
    {
        Vector3Int cellpos = SceneManager.Ins.Scene.MapManager.GetCellPos(BuildingOutPosition, TileNum);
        transform.position = SceneManager.Ins.Scene.MapManager.GetCellWorldPos(cellpos, TileNum);
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

    IEnumerator Enum_Leave()
    {
        yield return null;
    }

    void StateInit_Leave()
    {
        particleMain.Play();
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

    void UintDestroy()
    {
        if (particleMain == null)
        {
            destroyDuration -= Time.deltaTime;
            spriteMainRenderer.material.SetFloat("_Fade", destroyDuration);
            outLineRenderer.material.SetFloat("_Fade", destroyDuration);
            if (destroyDuration <= 0.0f)
                gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Env"))
        {
            if (IsSetConFidenceByEnv)
                return;

            if(!collision.GetComponentInParent<ClimateType>().Type.Equals(ENV_TYPE.NONE))
            {
                IsSetConFidenceByEnv = true;
                CurrentEnv = collision.GetComponentInParent<ClimateType>().gameObject;
                if (CameraManager.Ins.IsCameraCompleteFilmed(CAMERA_TYPE.ENV))
                {
                    Debug.Log("제대로 된 신고로 인한 회복");
                    Confidence += Random.Range(1, 6);
                }
                else
                {
                    Debug.Log("제대로 안된 신고로 인한 하락");
                    Confidence -= Random.Range(1, 10);
                }
            }
        }

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

        if (collision.tag == "Pol")
        {
            IsPolluting = true;
            StartCoroutine(Polluting());
        }
    }

    IEnumerator Polluting()
    {
        while(IsPolluting)
        {
            Confidence -= Random.Range(0.1f, 1f);
            Debug.Log("오염");
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsPolluting = false;
    }
}
