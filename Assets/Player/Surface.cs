using System;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public Action OnReachedSurface;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.tag == "Player")
        {
            OnReachedSurface?.Invoke();
        }
    }
}
