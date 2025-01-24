using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    private Transform playerTransform;
    public Vector2 baseOffset = new Vector2(1.0f, 0);
    private float spacing = 0.6f;
    public List<GameObject> npcs = new List<GameObject>();

    private void Start()
    {
        playerTransform = this.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            collision.transform.SetParent(playerTransform);

            int attachedCount = playerTransform.childCount - 1;
            Vector2 offset = new Vector2(baseOffset.x + (attachedCount * spacing), baseOffset.y);
            collision.transform.localPosition = offset;

            npcs.Add(collision.gameObject);
            collision.GetComponent<CreatureOxygen>().OnDepleted += OnNpcDied;
            this.GetComponent<PlayerMovement>().OnIncrementCollectedPeopleDown();
        }
    }

    private void OnNpcDied(GameObject @object)
    {
        @object.transform.SetParent(null);
        @object.GetComponent<CapsuleCollider2D>().enabled = false;
        @object.GetComponent<SpriteRenderer>().color = Color.red;
        npcs.Remove(@object);

        for (int i = 0; i < npcs.Count; i++)
        {
            Vector2 offset = new Vector2(baseOffset.x + (i * spacing), baseOffset.y);
            npcs[i].transform.localPosition = offset;
        }
    }

}