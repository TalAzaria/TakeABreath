using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float drag = 0.9f;
    public float downwardSpeed = 2f;
    public bool enableDownwardMovement = true;
    private Vector2 currentVelocity;

    private float originalMoveSpeed;


    void Start()
    {
        originalMoveSpeed = moveSpeed;

    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = input.normalized;

        currentVelocity = Vector2.Lerp(currentVelocity, input * moveSpeed, Time.deltaTime * 5f);

        if (float.IsNaN(currentVelocity.x) || float.IsNaN(currentVelocity.y))
        {
            currentVelocity = Vector2.zero;
        }

        if (enableDownwardMovement && currentVelocity.magnitude < 0.1f)
        {
            currentVelocity += Vector2.down * (downwardSpeed * Time.deltaTime);
        }

        currentVelocity *= drag;

        currentVelocity = Vector2.ClampMagnitude(currentVelocity, moveSpeed);

        transform.Translate(currentVelocity * Time.deltaTime);
    }

    private void ChangeDownwardSpeedBasedOnPeople(int peopleCountChange)
    {
        moveSpeed = originalMoveSpeed - peopleCountChange;
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