using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleLogic : MonoBehaviour
{
    [SerializeField] private float startingOxygen = 100f;
    [SerializeField]  private float oxygenInsideBubble;
    public float oxygenDepletionRate = 1f;
    private float minimumSize = 0.02f;
    public float scalingDown = 5f;
    public float shrinkAmount = 0.05f;
    private Vector2 originalScale;

    public GameObject Player;
    CreatureOxygen playerOxygen;
    List<GameObject> npcs = new List<GameObject>();

    public float oxygenBoostRate = 0.5f;

    private void Start()
    {
        oxygenInsideBubble = startingOxygen;
        originalScale = transform.localScale;

        playerOxygen = Player.GetComponent<CreatureOxygen>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered the bubble");
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            npcs = Player.GetComponent<NPCLogic>().npcs;

            if (playerOxygen != null)
            {
                playerOxygen.changeRatePerSecond = 0;
                playerOxygen.Levels += oxygenBoostRate;
            }
            if(npcs.Count > 0)
            {
                foreach (GameObject npc in npcs)
                {
                    CreatureOxygen npcOxygen = npc.GetComponent<CreatureOxygen>();
                    npcOxygen.changeRatePerSecond = 0;
                    npcOxygen.Levels += oxygenBoostRate;
                }

                oxygenInsideBubble -= oxygenDepletionRate * Time.deltaTime * npcs.Count;
                transform.localScale = (oxygenInsideBubble / startingOxygen) * originalScale;
            }
            else
            {
                oxygenInsideBubble -= oxygenDepletionRate * Time.deltaTime;
                transform.localScale = (oxygenInsideBubble / startingOxygen) * originalScale;
            }




            if (oxygenInsideBubble <= minimumSize)
            {
                Destroy(gameObject);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            npcs = Player.GetComponent<NPCLogic>().npcs;

            if (playerOxygen != null)
            {
                playerOxygen.SetChangeRateToDefault();

                foreach (GameObject npc in npcs)
                {
                    CreatureOxygen npcOxygen = npc.GetComponent<CreatureOxygen>();
                    npcOxygen.SetChangeRateToDefault();
                }
            }
        }
    }

}
