using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float drag = 0.9f;
    public float downwardSpeed = 2f;
    public bool enableDownwardMovement = true;
    private Vector2 currentVelocity = Vector2.zero;
    private float originalMoveSpeed;
    private float originalDownwardsSpeed;
    private int maxToHold = 4;
    [SerializeField] private Transform Swimmer;

    [Header("Bubble Emitter")]
    [SerializeField] private Transform BubbleEmitter;
    [SerializeField] private Vector3 PositionUpperLeft = new Vector3(-0.2f, 1, 0);
    [SerializeField] private Vector3 PositionUpperRight = new Vector3(0.2f, 1, 0);
    [SerializeField] private Vector3 PositionLowerLeft = new Vector3(-0.2f, -1, 0);
    [SerializeField] private Vector3 PositionLowerRight = new Vector3(0.2f, -1, 0);

    void Start()
    {
        originalMoveSpeed = moveSpeed;
        originalDownwardsSpeed = downwardSpeed;

    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        currentVelocity = Vector2.Lerp(currentVelocity, input * moveSpeed, Time.deltaTime * 5f);

        if (enableDownwardMovement && currentVelocity.magnitude < 0.1f)
            currentVelocity += Vector2.down * (downwardSpeed * Time.deltaTime);

        currentVelocity *= drag;
        currentVelocity = Vector2.ClampMagnitude(currentVelocity, moveSpeed);

        transform.Translate(currentVelocity * Time.deltaTime);
        SyncSwimmerOrientation(input);
    }


    Vector3 euler = Vector3.zero;
    private void SyncSwimmerOrientation(Vector2 input)
    {
        if (input.y > 0)
        {
            euler = Swimmer.localEulerAngles;
            euler.z = 0;
            Swimmer.localEulerAngles = euler;
        }
        else if (input.y < 0)
        {
            euler = Swimmer.localEulerAngles;
            euler.z = 180;
            Swimmer.localEulerAngles = euler;
        }

        if (input.x > 0)
        {
            euler = Swimmer.localEulerAngles;
            euler.y = (euler.z == 0) ? 180 : 0;
            Swimmer.localEulerAngles = euler;
        }
        else if (input.x < 0)
        {
            euler = Swimmer.localEulerAngles;
            euler.y = (euler.z == 0) ? 0 : 180;
            Swimmer.localEulerAngles = euler;
        }

        if ((euler.y == 0) && (euler.z == 0))
        {
            BubbleEmitter.localPosition = PositionUpperRight;
        }
        else if ((euler.y == 180) && (euler.z == 0))
        {
            BubbleEmitter.localPosition = PositionUpperLeft;
        }
        else if ((euler.y == 0) && (euler.z == 180))
        {
            BubbleEmitter.localPosition = PositionLowerRight;
        }
        else if ((euler.y == 180) && (euler.z == 180))
        {
            BubbleEmitter.localPosition = PositionLowerLeft;
        }
    }


    private void ChangeDownwardSpeedBasedOnPeople(int PeopleCount)
    {
        switch (PeopleCount)
        {
            case 0:
                moveSpeed = originalMoveSpeed;
                downwardSpeed = originalDownwardsSpeed;
                break;
            case 1:
                moveSpeed = 4f;
                downwardSpeed = 3f;
                break;
            case 2:
                moveSpeed = 3f;
                downwardSpeed = 4f;
                break;

            case 3:
                moveSpeed = 2f;
                downwardSpeed = 5f;
                break;

            default:
                if (PeopleCount >=maxToHold )
                {
                    moveSpeed = 1f;
                    downwardSpeed = 6f;
                    Debug.Log("Maximum npcs. Can't move upward");
                    
                }
                else
                {
                    Debug.Log($"There are {PeopleCount} players.");
                    moveSpeed = originalMoveSpeed; 
                    downwardSpeed = originalDownwardsSpeed;
                }
                break;
        }
    }

    public void OnCollectedChange(int CountPeople)
    {
        ChangeDownwardSpeedBasedOnPeople(CountPeople);
    }

    
}