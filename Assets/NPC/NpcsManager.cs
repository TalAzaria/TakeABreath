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
        npc.GetComponent<SpriteRenderer>().color = Color.red;
        if (npcOxygenList.Count == 0)
        {
            Debug.Log("All NPCs are dead. Game Over!");
            gameOverManager?.EndGame();
        }
    }
}
