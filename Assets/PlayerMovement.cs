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
        float speedMultiplier = 1f;
        float downwardSpeedMultiplier = 1f;

        switch (PeopleCount)
        {
            case 0:
                speedMultiplier = 1f; 
                downwardSpeedMultiplier = 1f;  
                break;
            case 1:
                speedMultiplier = 0.8f;  
                downwardSpeedMultiplier = 1.5f;  
                break;
            case 2:
                speedMultiplier = 0.6f;  
                downwardSpeedMultiplier = 2f;  
                break;
            case 3:
                speedMultiplier = 0.45f;  
                downwardSpeedMultiplier = 2.5f;  
                break;
            default:
                if (PeopleCount >= maxToHold)
                {
                    speedMultiplier = 0.35f; 
                    downwardSpeedMultiplier = 3f;  
                    Debug.Log("Maximum NPCs. Can't move upward");
                }
                else
                {
                    Debug.Log($"There are {PeopleCount} players.");
                    speedMultiplier = 1f;  // Default to original speed
                    downwardSpeedMultiplier = 1f;  // Default downward speed
                }
                break;
        }

        // Apply the multiplier to the move and downward speed
        moveSpeed = originalMoveSpeed * speedMultiplier;
        downwardSpeed = originalDownwardsSpeed * downwardSpeedMultiplier;
    }


    public void OnCollectedChange(int CountPeople)
    {
        ChangeDownwardSpeedBasedOnPeople(CountPeople);
    }

    
}