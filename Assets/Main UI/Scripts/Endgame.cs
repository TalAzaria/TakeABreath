using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;



public class Endgame : MonoBehaviour
{
    [SerializeField] private NpcsManager NpcManager;
    [SerializeField] private List<TextMeshProUGUI> RescuedTextDisplays = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> DrownedTextDisplays = new List<TextMeshProUGUI>();

    TextMeshPro ScoreText;

    public GameObject Cat1;
    public GameObject Cat2;
    public GameObject Cat22;
    public GameObject Cat3;
    public GameObject Dog1;
    public GameObject Dog2;
    public GameObject Dog22;
    public GameObject Dog3;
    public GameObject Duck1;
    public GameObject Duck11;
    public GameObject Duck2;

    int Cat2Counter = 0;
    int Dog2Counter = 0;
    int Duck1Counter = 0;

    public Scores score;

    [ContextMenu("OnEndgame")]
    public void OnEndgame()
    {
        gameObject.SetActive(true);
        NpcManager = NpcsManager.Instance;

        for (int i = 0; i < RescuedTextDisplays.Count; i++)
        {
            switch (NpcManager.RescuedNPCCounters[i].ToString())
            {
                case "Cat1":
                        Cat1.gameObject.SetActive(true);
                        break;
                case "Cat2":
                    if(Cat2Counter == 0)
                    {
                        Cat2.gameObject.SetActive(true);
                        Cat2Counter = 1;

                    }

                    if (Cat2Counter == 1)
                        Cat22.gameObject.SetActive(true);

                    break;

                case "Cat3":
                    Cat3.gameObject.SetActive(true);

                    break;

                case "Dog1":
                    Dog1.gameObject.SetActive(true);

                    break;

                case "Dog2":
                    if (Dog2Counter == 0)
                    {
                        Dog2.gameObject.SetActive(true);
                        Dog2Counter = 1;

                    }

                    if (Dog2Counter == 1)
                        Dog22.gameObject.SetActive(true);
                    break;

                case "Dog3":
                    Dog3.gameObject.SetActive(true);

                    break;

                case "Duck1":
                    if (Duck1Counter == 0)
                    {
                        Duck1.gameObject.SetActive(true);
                        Duck1Counter = 1;

                    }

                    if (Duck1Counter == 1)
                        Duck11.gameObject.SetActive(true);
                    break;

                case "Duck2":
                    Duck2.gameObject.SetActive(true);

                    break;

                case "Duck3":
                    Duck2.gameObject.SetActive(true);

                    break;



            }


           // RescuedTextDisplays[i].text = NpcManager.RescuedNPCCounters[i].ToString();
           // DrownedTextDisplays[i].text = NpcManager.DeadNPCCounters[i].ToString();
        }

        int sumCountingReqscued = 0;

        foreach (int counter in NpcManager.RescuedNPCCounters)
        {
            sumCountingReqscued += counter;
        }

        score.sumScore(sumCountingReqscued);

        ScoreText.text = "Rescued: " + sumCountingReqscued.ToString();

    }
}
