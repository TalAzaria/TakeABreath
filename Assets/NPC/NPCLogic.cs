using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    public Action<int> OnReachSurfaceWithNpc;
    private Transform playerTransform;
    public Vector2 baseOffset = new Vector2(1.0f, 0);
    private float spacing = 0.6f;
    public List<GameObject> npcs = new List<GameObject>();
    public Surface surface;
    private void Start()
    {
        surface = Surface.Instance;
        surface.OnReachedSurface += OnReachSurface;
        playerTransform = this.transform;
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space) && npcs.Count > 0)
        {
            DropOneNPC();
        }
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
            this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);
        }
    }
    private void DropOneNPC()
    {
       
            GameObject npcToDrop = npcs[npcs.Count - 1];

            npcToDrop.transform.SetParent(null);

            npcToDrop.transform.position = playerTransform.position + Vector3.right * 2f;
            Collider2D npcCollider = npcToDrop.GetComponent<Collider2D>();
            if (npcCollider != null)
            {
                npcCollider.enabled = false;
                StartCoroutine(EnableNpcColliderAfterDelay(npcCollider, 2f)); 
            }

            npcToDrop.GetComponent<CreatureOxygen>().OnDepleted -= OnNpcDied;

            npcs.RemoveAt(npcs.Count - 1);

            this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);

            Debug.Log($"Dropped NPC Remaining NPCs: {npcs.Count}");
        
    }
    private IEnumerator EnableNpcColliderAfterDelay(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collider != null)
        {
            collider.enabled = true;
        }
    }

    private void OnReachSurface()
    {
        if (npcs.Count>0)
        {
            OnReachSurfaceWithNpc?.Invoke(npcs.Count);
            for (int i = npcs.Count-1; i >= 0 ; i--)
            {
                GameObject npc = npcs[i];
                DropOneNPC();
                Destroy(npc);
            }
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
        this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);
    }

}