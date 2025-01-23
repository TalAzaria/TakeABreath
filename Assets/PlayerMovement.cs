using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float drag = 0.9f;
    public float downwardSpeed = 2f;
    public bool enableDownwardMovement = true;
    private Vector2 currentVelocity;

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = input.normalized;
        currentVelocity = Vector2.Lerp(currentVelocity, input * moveSpeed, Time.deltaTime * 5f);

        if (enableDownwardMovement && currentVelocity.magnitude < 0.1f)
        {
            currentVelocity += Vector2.down * (downwardSpeed * Time.deltaTime);
        }

        currentVelocity *= drag;
        transform.Translate(currentVelocity * Time.deltaTime);
    }

    private void ChangeDownwardSpeedBasedOnPeople(int peopleCountChange)
    {
        downwardSpeed += peopleCountChange * 1f;
        drag += peopleCountChange * 0.05f;
        moveSpeed += peopleCountChange * 0.5f;

        downwardSpeed = Mathf.Max(downwardSpeed, 0f);
        drag = Mathf.Max(drag, 0f);
        moveSpeed = Mathf.Max(moveSpeed, 0f);

        Debug.Log("Downward speed: " + downwardSpeed + ", Drag: " + drag);
    }

    public void OnIncrementCollectedPeopleDown()
    {

        ChangeDownwardSpeedBasedOnPeople(1);
    }

    public void OnRemoveAllCollectedPeople(int amountRemoved)
    {
        ChangeDownwardSpeedBasedOnPeople(-amountRemoved);
    }
}