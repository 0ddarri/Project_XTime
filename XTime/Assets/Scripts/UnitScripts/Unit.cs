using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UNIT_STATE // �ش� ������ �������� �ȶ�������
{
    NORMAL,
    LEAVED
}

public enum MOVEMENT_STATE // ������ ����, ���ĺ��� �� ����Ѵ�
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
    
    [Header ("State")]
    [SerializeField] float Reliability; // �ŷڵ�
    [SerializeField] UNIT_STATE UnitState;
    [SerializeField] MOVEMENT_STATE MovementState;
    [Header ("Status")]
    [SerializeField] float MovementSpeed;

    float MoveTime = 0.0f;

    public void Initialize()
    {
        Reliability = 100;
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
        MoveTime += Time.deltaTime;
        Vector2 dir = MoveDirection.GetRandomDirection();
        Vector2 newPos = Rb2D.position + dir * MovementSpeed * Time.fixedDeltaTime;
        Rb2D.MovePosition(newPos);
    }

    private void FixedUpdate()
    {
        RandomMove();
    }
}
