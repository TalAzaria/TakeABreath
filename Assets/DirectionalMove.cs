using System;
using System.Collections;
using UnityEngine;

public class DirectionalMove : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] private Vector3 targetPositionOffset;
    [SerializeField] private float moveRate = 1;

    private void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(Move());
        
    }

    private IEnumerator Move()
    {
        while (true)
        {
            float targetLerpTime = Time.time + moveRate / 2;
            while (Time.time <= targetLerpTime)
            {
                transform.position = Vector3.Lerp(transform.position, originalPosition + targetPositionOffset, Time.time);
                yield return 0;
            }

            targetLerpTime = Time.time + moveRate / 2;
            while (Time.time <= targetLerpTime)
            {
                transform.position = Vector3.Lerp(transform.position, originalPosition - targetPositionOffset, Time.time);
                yield return 0;
            }

            yield return 0;
        }
    }
}
