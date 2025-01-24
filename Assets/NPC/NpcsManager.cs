using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcsManager : MonoBehaviour
{
    public GameOverManager gameOverManager;
    private List<CreatureOxygen> npcOxygenList = new();
    
    private void Start()
    {
        npcOxygenList = GetComponentsInChildren<CreatureOxygen>().ToList();
        foreach (CreatureOxygen creature in npcOxygenList)
        {
            creature.OnDepleted += OnCreatureOxygenDepleted;
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
    }
}
