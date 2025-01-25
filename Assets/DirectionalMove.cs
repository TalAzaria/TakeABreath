using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class DirectionalMove : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] private Vector3 targetPositionOffset;
    [SerializeField] private float moveRate = 1;
    [SerializeField] private float startDelay = 0;

    private void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            float startingTime = Time.time;
            float targetLerpTime = startingTime + moveRate;
            while (Time.time <= targetLerpTime)
            {
                transform.position = Vector3.Lerp(originalPosition, originalPosition + targetPositionOffset, (Time.time - startingTime) / moveRate);
                yield return 0;
            }

            startingTime = Time.time;
            targetLerpTime = startingTime + moveRate;
            while (Time.time <= targetLerpTime)
            {
                transform.position = Vector3.Lerp(originalPosition + targetPositionOffset, originalPosition, (Time.time - startingTime) / moveRate);
                yield return 0;
            }
        }
    }
}
