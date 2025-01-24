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
                // Logic for when there is 1 player
                Debug.Log("There is 1 player.");
                //moveSpeed = originalMoveSpeed * 0.8f;
                moveSpeed = 4f;
                downwardSpeed = 3f;
                break;
            case 2:
                // Logic for when there are 2 players
                Debug.Log("There are 2 players."); 
                //moveSpeed = originalMoveSpeed * 0.6f; 
                moveSpeed = 3f;
                downwardSpeed = 4f;
                break;

            case 3:
                // Logic for when there are 3 players
                Debug.Log("There are 3 players.");
                //moveSpeed = originalMoveSpeed * 0.4f; 
                moveSpeed = 2f;
                downwardSpeed = 5f;
                break;

            default:
                if (PeopleCount >=maxToHold )
                {
                    moveSpeed = 0.5f;
                    downwardSpeed = 6f;
                    Debug.Log("Maximum players. Can't move upward");
                    
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