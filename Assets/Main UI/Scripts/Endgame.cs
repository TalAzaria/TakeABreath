using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;



public class Endgame : MonoBehaviour
{
    [SerializeField] private NpcsManager NpcManager;
    [SerializeField] private List<TextMeshProUGUI> RescuedTextDisplays = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> DrownedTextDisplays = new List<TextMeshProUGUI>();


    [ContextMenu("OnEndgame")]
    public void OnEndgame()
    {
        gameObject.SetActive(true);
        NpcManager = NpcsManager.Instance;

        for (int i = 0; i < RescuedTextDisplays.Count; i++)
        {
            RescuedTextDisplays[i].text = NpcManager.RescuedNPCCounters[i].ToString();
            DrownedTextDisplays[i].text = NpcManager.DeadNPCCounters[i].ToString();
        }
    }
}
