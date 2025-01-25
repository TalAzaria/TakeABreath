using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    public static NPCLogic Instance;
    public Action<int> OnReachSurfaceWithNpc;
    private Transform playerTransform;
    public GameObject pressSpaceToDrop;

    public List<GameObject> npcs = new List<GameObject>();

    private Transform[] npcSlots;
    public Surface surface;
    bool isDelay = false;

    private int NpcCountStart;

    public float wobbleStrength = 0.25f;
    public float wobbleSpeed = 1.5f;

    private GameObject npcInsideBubble;

    private CapsuleCollider2D playerCollider;

    private void Awake()
    {
        Instance = this;
    }

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

        CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();

        foreach (var collider in colliders)
        {
            if (collider.isTrigger)
            {
                playerCollider = collider;
                break; 
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && npcs.Count > 0)
        {
            DropOneNPC();
        }

        if (npcs.Count == 0)
        {
            pressSpaceToDrop.SetActive(false);

        }

        for (int i = 0; i < npcs.Count; i++)
        {
            GameObject npc = npcs[i];
            if (npc != null)
            {
                WobbleNPC(npc, i);
            }
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
                collision.GetComponent<CreatureOxygen>().isHoldingOnNpc = true;
                collision.GetComponent<CreatureOxygen>().OnDepleted += OnNpcDied;
                this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);
                isDelay = true;
                StartCoroutine(EnableTakingMorePlayersAfterDelay());
                ResizeColliderToFitNPCs();

                pressSpaceToDrop.SetActive(true);
            }
        }
    }


    private void DropOneNPC()
    {
        GameObject npcToDrop = npcs[npcs.Count - 1];
        Transform npcSlot = npcSlots[npcs.Count - 1];
        npcToDrop.transform.SetParent(null);
        npcToDrop.transform.position = transform.position + Vector3.down * 1.3f;;
        //npcToDrop.transform.position = npcSlot.position + Vector3.down * 1.3f;

        Collider2D npcCollider = npcToDrop.GetComponent<Collider2D>();
        if (npcCollider != null)
        {
            npcCollider.enabled = false;
            StartCoroutine(EnableNpcColliderAfterDelay(npcCollider, 2f));
        }

        npcToDrop.GetComponent<CreatureOxygen>().OnDepleted -= OnNpcDied;
        npcToDrop.GetComponent<CreatureOxygen>().isHoldingOnNpc = false;
        npcInsideBubble = npcToDrop;
        InsideBubble(npcInsideBubble);

        npcs.RemoveAt(npcs.Count - 1);
        this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);

        // Debug.Log($"Dropped NPC. Remaining NPCs: {npcs.Count}");

        ResizeColliderToFitNPCs();

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
                NpcsManager.Instance.OnNPCRescued(npc);
                Destroy(npc);
            }
        }
    }

    private void OnNpcDied(GameObject npc)
    {
        npc.transform.SetParent(null);
        npcs.Remove(npc);
        RepositionNPCs();
        this.GetComponent<PlayerMovement>().OnCollectedChange(npcs.Count);
        ResizeColliderToFitNPCs();
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

    private void WobbleNPC(GameObject npc, int npcIndex)
    {
        Transform npcTransform = npc.transform;
        Transform slotTransform = npcSlots[npcIndex];

        Vector2 playerMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (playerMovement.magnitude > 0.1f)
        {
            float wobbleAmount = Mathf.Sin(Time.time * wobbleSpeed + npcIndex) * wobbleStrength;
            Vector3 wobblePosition = slotTransform.position + new Vector3(wobbleAmount, wobbleAmount, 0);
            npcTransform.position = Vector3.Lerp(npcTransform.position, wobblePosition, Time.deltaTime * wobbleSpeed);
        }
        else
        {
            npcTransform.position = Vector3.Lerp(npcTransform.position, slotTransform.position, Time.deltaTime * wobbleSpeed);
        }
    }

    public void InsideBubble(GameObject npc)
    {
        if (npc != null)
        {
            CapsuleCollider2D capsule = npc.GetComponent<CapsuleCollider2D>();
            if (capsule == null)
            {
                Debug.LogError("No CapsuleCollider2D attached to this NPC.");
                return;
            }

            Collider2D[] overlappingColliders = Physics2D.OverlapCapsuleAll(
                capsule.bounds.center,
                capsule.size,
                capsule.direction,
                0
            );

            int x = 0;

            foreach (var collider in overlappingColliders)
            {
                if (collider != capsule)
                {
                    if (collider.TryGetComponent<BubbleLogic>(out BubbleLogic bubbleLogic))
                    {
                        collider.GetComponent<BubbleLogic>().npcIsInsideLogic(npc);
                        x = 1;
                    }

                }
            }

            if (x == 1)
            {
                npc.GetComponent<CreatureOxygen>().isInsideBubble = true;
            }
            else
            {
                npc.GetComponent<CreatureOxygen>().isInsideBubble = false;
                npc.GetComponent<CreatureOxygen>().SetChangeRateToDefault();
            }
        }
    }
    private void ResizeColliderToFitNPCs()
    {
        if (playerCollider == null) return;


            float baseHeight = playerCollider.size.y;

            if (npcs.Count == 0)
            {
                playerCollider.size = new Vector2(1f, baseHeight);
                playerCollider.offset = Vector2.zero;
                return;
            }

            float leftmost = float.MaxValue;
            float rightmost = float.MinValue;

            for (int i = 0; i < npcs.Count; i++)
            {
                Transform slot = npcSlots[i];
                if (slot != null)
                {
                    float relativeX = slot.localPosition.x;
                    leftmost = Mathf.Min(leftmost, relativeX);
                    rightmost = Mathf.Max(rightmost, relativeX);
                }
            }

            float newWidth = rightmost - leftmost + 1.2f;
            float newCenterX = (leftmost + rightmost) / 2f;

            playerCollider.size = new Vector2(newWidth, baseHeight);
            playerCollider.offset = new Vector2(newCenterX, 0f);
    }



}
