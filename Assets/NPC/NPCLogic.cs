using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    public GameOverManager gameOverManager;

    public Action<int> OnReachSurfaceWithNpc;
    private Transform playerTransform;

    public List<GameObject> npcs = new List<GameObject>();
    public List<GameObject> deadNps = new List<GameObject>();

    private Transform[] npcSlots;
    public Surface surface;
    bool isDelay = false;

    private int NpcCountStart;

    private void Start()
    {
        surface = Surface.Instance;
        surface.OnReachedSurface += OnReachSurface;
        playerTransform = this.transform;

        npcSlots = new Transform[4];
        npcSlots[0] = playerTransform.Find("NpcHold/Left/1");
        npcSlots[1] = playerTransform.Find("NpcHold/Left/2");
        npcSlots[2] = playerTransform.Find("NpcHold/Right/3");
        npcSlots[3] = playerTransform.Find("NpcHold/Right/4");
        isDelay = false;

        NpcCountStart = npcs.Count;
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
            if (npcs.Count < npcSlots.Length && npcSlots[npcs.Count] != null && !isDelay)
            {
                Transform slot = npcSlots[npcs.Count];
                collision.transform.SetParent(slot);
                collision.transform.localPosition = Vector3.zero;
                npcs.Add(collision.gameObject);
                collision.GetComponent<CreatureOxygen>().OnDepleted += OnNpcDied;
                this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);
                isDelay = true;
                StartCoroutine(EnableTakingMorePlayersAfterDelay());
            }
        }
    }


    private void DropOneNPC()
    {
        GameObject npcToDrop = npcs[npcs.Count - 1];
        Transform npcSlot = npcSlots[npcs.Count - 1];
        npcToDrop.transform.SetParent(null);
        npcToDrop.transform.position = npcSlot.position + Vector3.down * 1.3f;

        Collider2D npcCollider = npcToDrop.GetComponent<Collider2D>();
        if (npcCollider != null)
        {
            npcCollider.enabled = false;
            StartCoroutine(EnableNpcColliderAfterDelay(npcCollider, 2f));
        }

        npcToDrop.GetComponent<CreatureOxygen>().OnDepleted -= OnNpcDied;

        npcs.RemoveAt(npcs.Count - 1);
        this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);

        // Debug.Log($"Dropped NPC. Remaining NPCs: {npcs.Count}");
        Debug.Log("slots " + npcs.Count);

    }

    private IEnumerator EnableNpcColliderAfterDelay(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    private IEnumerator EnableTakingMorePlayersAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        isDelay = false;
    }

    private void OnReachSurface()
    {
        if (npcs.Count > 0)
        {
            OnReachSurfaceWithNpc?.Invoke(npcs.Count);
            for (int i = npcs.Count - 1; i >= 0; i--)
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
        deadNps.Add(@object);

        RepositionNPCs();
        this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);
        if (deadNps.Count == NpcCountStart)
        {
            Debug.Log("All NPCs are dead. Game Over!");
            if (gameOverManager != null)
            {
                gameOverManager.TriggerGameOver();
            }
        }
    }

    private void RepositionNPCs()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            Transform slot = npcSlots[i];
            npcs[i].transform.position = slot.position;
            npcs[i].transform.localPosition = Vector3.zero;
        }
    }
}