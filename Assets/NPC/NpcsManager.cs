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
    public List<int> NPCCounters = new List<int>();

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
            NPCCounters.Add(0);
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
    }

    public void OnNPCRescued(GameObject npc)
    {
        NPCVisual npcVisual = npc.GetComponent<NPCVisual>();
        NPCCounters[(int)(npcVisual.NPCType)]++;
    }
}
