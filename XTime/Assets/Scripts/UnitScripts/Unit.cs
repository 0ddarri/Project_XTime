using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum UNIT_STATE // 해당 유닛이 떠났는지 안떠났는지
{
    NORMAL,
    LEAVED
}

public enum MOVEMENT_STATE // 움직임 상태, 기후별로 다 써야한다
{
    NORMAL,
    FOG,
    YELLOW_DUST
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
    [SerializeField] Transform RayTransform;
    [SerializeField] float RayLength;
    
    [Header ("State")]
    [SerializeField] float Reliability; // 신뢰도
    [SerializeField] UNIT_STATE UnitState;
    [SerializeField] MOVEMENT_STATE MovementState;
    [Header ("Status")]
    [SerializeField] float MovementSpeed;
    [SerializeField] float ChangeMoveTimeMin;
    [SerializeField] float ChangeMoveTimeMax;

    [SerializeField] List<Vector3> nearPos = new List<Vector3>();

    float CurrentMoveTime = 0.0f;
    float MoveTime = 0.0f;
    Vector2 MoveDir;

    float t = 0.0f;
    Vector3 BeforeTilePos;
    Vector3 ResultNearPos;
    [SerializeField] GameObject BeforeTileVisualize;

    public void Initialize()
    {
        Reliability = 100;
        MoveTime = Random.Range(ChangeMoveTimeMin, ChangeMoveTimeMax);
        MoveDir = MoveDirection.GetRandomDirection();
        BeforeTilePos = SceneManager.Ins.Scene.mapManager.GetCellWorldPos(SceneManager.Ins.Scene.mapManager.GetCellPos(transform.position));
        ResultNearPos = BeforeTilePos;
    }

    private void Start()
    {
        Initialize();
    }

    void Move()
    {
        Vector2 currentPos = Rb2D.position;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 inputVector = new Vector2(h, v);
        Vector2 isoVector = new Vector2(inputVector.x - inputVector.y, (inputVector.x + inputVector.y) / 2);
        //inputVector = Vector2.ClampMagnitude(inputVector, 1);
        isoVector = Vector2.ClampMagnitude(isoVector, 1);
        //Vector2 movement = inputVector * MovementSpeed;
        Vector2 movement = isoVector * MovementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        Rb2D.MovePosition(newPos);
    }

    void RandomMove()
    {
        CurrentMoveTime += Time.deltaTime;
        if(CurrentMoveTime > MoveTime)
        {
            CurrentMoveTime = 0.0f;
            MoveTime = Random.Range(ChangeMoveTimeMin, ChangeMoveTimeMax);
            MoveDir = MoveDirection.GetRandomDirection();
        }

        Vector2 newPos = Rb2D.position + MoveDir * MovementSpeed * Time.fixedDeltaTime;
        Rb2D.MovePosition(newPos);
    }

    //Vector3 cellToWorldPos = Vector3.zero;
    //void TileMove()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        MapManager manager = SceneManager.Ins.Scene.mapManager;

    //        Vector3 camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        camPos.z = 0;
    //        Vector3Int cellPos = SceneManager.Ins.Scene.mapManager.GetCellPos(camPos);
    //        Debug.Log(cellPos);
    //        if (manager.Tile.HasTile(cellPos))
    //        {
    //            Debug.Log("asdfasdfasdfasdfasdfasdfasdfasdfasdfasdf : " + cellPos);
    //            cellToWorldPos = manager.GetCellWorldPos(cellPos);
    //            Debug.Log(cellToWorldPos);
    //            cellToWorldPos.y += 0.25f;
    //        }
    //    }
    //    transform.position = Vector3.Lerp(transform.position, cellToWorldPos, Time.deltaTime * 20);
    //}

    void AutoTileMove()
    {
        Vector3 CheckPos = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
        nearPos = SceneManager.Ins.Scene.mapManager.GetNearMovableWorldCellPos(CheckPos);

        for(int i = 0; i < nearPos.Count; i++)
        {
            if(!Mathf.Approximately(nearPos[i].z, -10000.0f))
            {
                Vector3 ResultNearPos = new Vector3(nearPos[i].x, nearPos[i].y + 0.25f, nearPos[i].z);
                Debug.Log("Near Pos : " + ResultNearPos);
                transform.position = Vector3.Lerp(transform.position, ResultNearPos, Time.deltaTime);
            }
        }
    }

    void StaticTileMove()
    {
        t += Time.deltaTime;
        if(t > 0.2f)
        {
            t = 0.0f;
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

    private void Update()
    {
        StaticTileMove();
        BeforeTileVisualize.transform.position = BeforeTilePos;
    }

    private void FixedUpdate()
    {
        //RandomMove();
    }
}
