using System;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public static Surface Instance = null;
    public Action OnReachedSurface;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.tag == "Player")
        {
            OnReachedSurface?.Invoke();
        }
    }
}
