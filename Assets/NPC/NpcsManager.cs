using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcsManager : MonoBehaviour
{
    public static NpcsManager Instance = null;
    public GameOverManager gameOverManager;
    private List<CreatureOxygen> npcOxygenList = new List<CreatureOxygen>();
    public List<NPCVisual> NPCVisuals = new List<NPCVisual>();
    public List<int> RescuedNPCCounters = new List<int>();
    public List<int> DeadNPCCounters = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        npcOxygenList = GetComponentsInChildren<CreatureOxygen>().ToList();
        foreach (CreatureOxygen creature in npcOxygenList)
        {
            creature.OnDepleted += OnCreatureOxygenDepleted;
        }
        
        NPCVisuals = GetComponentsInChildren<NPCVisual>().ToList();
        foreach (NPCVisual npc in NPCVisuals)
        {
            RescuedNPCCounters.Add(0);
            DeadNPCCounters.Add(0);
        }
    }

    private void OnCreatureOxygenDepleted(GameObject npc)
    {
        npc.GetComponent<CapsuleCollider2D>().enabled = false;
        CreatureOxygen creature = npc.GetComponent<CreatureOxygen>();
        creature.bubblesParticles.Stop();
        npcOxygenList.Remove(creature);
        if (npcOxygenList.Count == 0)
            gameOverManager?.EndGame();

        NPCVisual npcVisual = npc.GetComponent<NPCVisual>();
        npcVisual.IsAlive = false;
        DeadNPCCounters[(int)(npcVisual.NPCType)]++;
    }

    public void OnNPCRescued(GameObject npc)
    {
        NPCVisual npcVisual = npc.GetComponent<NPCVisual>();
        npcVisual.IsRescued = true;
        RescuedNPCCounters[(int)(npcVisual.NPCType)]++;
    }


    public void OnEndgame()
    {

        for (int i = 0; i < NPCVisuals.Count; i++)
        {
            if (!NPCVisuals[i].IsRescued && NPCVisuals[i].IsAlive)
            {
                OnCreatureOxygenDepleted(NPCVisuals[i].gameObject);
            }

        }

    }
}
