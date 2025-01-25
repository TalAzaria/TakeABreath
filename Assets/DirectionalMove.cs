using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class DirectionalMove : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] private Vector3 targetPositionOffset;
    [SerializeField] private float moveRate = 1;

    private void Start()
    {
        moveRate *= 5;
        originalPosition = transform.position;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            float startingTime = Time.time;
            float targetLerpTime = startingTime + moveRate;
            while (Time.time <= targetLerpTime)
            {
                print("go up");
                transform.position = Vector3.Lerp(transform.position, originalPosition + targetPositionOffset, (Time.time - startingTime) / moveRate);
                yield return 0;
            }

            print("go down");
            startingTime = Time.time;
            targetLerpTime = startingTime + moveRate;
            while (Time.time <= targetLerpTime)
            {
                transform.position = Vector3.Lerp(transform.position, originalPosition, (Time.time - startingTime) / moveRate);
                yield return 0;
            }
            
            print("now what?");
        }
    }
}
