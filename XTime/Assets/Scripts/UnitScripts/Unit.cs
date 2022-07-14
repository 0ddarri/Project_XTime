using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Unit : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] Rigidbody2D Rb2D;
    
    [Header ("State")]
    [SerializeField] float Reliability; // 신뢰도
    [SerializeField] UNIT_STATE UnitState;
    [SerializeField] MOVEMENT_STATE MovementState;
    [Space (5.0f)]
    [SerializeField] float MovementSpeed;

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
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * MovementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        Rb2D.MovePosition(newPos);
    }

    private void FixedUpdate()
    {
        Move();
    }
}
