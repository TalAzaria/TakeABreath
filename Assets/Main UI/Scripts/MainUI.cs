using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class MainUI : MonoBehaviour
{
    public static MainUI Instance = null;
    private int RescuedCount = 0;
    [SerializeField] Text RescuedDisplay;
    [SerializeField] Animator RescuedAnimator;
    [SerializeField] float RescuedAnimationDelay = 0.5f;
    private float NPCsOxygenLeft = 100;
    [SerializeField] Text NPCsOxygenDisplay;
    [SerializeField] Animator NPCsOxygenAnimator;
    [SerializeField] int ColorThresholdPercent = 25;
    [SerializeField] Color OxygenOkColor;
    [SerializeField] Color OxygenRunningOutColor;
    [SerializeField] Animator Vignette;
    [SerializeField] CreatureOxygen PlayerOxygen;
    [SerializeField] Endgame EndgamePopup;



    private void Awake()
    {
        Instance = this;
        RescuedDisplay.text = "x" + RescuedCount.ToString();
        NPCsOxygenDisplay.text = NPCsOxygenLeft.ToString() + "%";
    }



    private void Start()
    {
        PlayerOxygen = GameObject.FindGameObjectWithTag("Player").GetComponent<CreatureOxygen>();
        PlayerOxygen.OnChanged += ReduceOxygen;
    }



    public void PlayerRescuedNPCs(int numberOfNPCs)
    {
        StartCoroutine(DelayedRescueFlash(numberOfNPCs));
    }



    IEnumerator DelayedRescueFlash(int numberOfNPCs)
    {
        yield return new WaitForSeconds(RescuedAnimationDelay);
        RescuedCount = RescuedCount + numberOfNPCs;
        RescuedDisplay.text = "x" + RescuedCount.ToString();
        RescuedAnimator.SetTrigger("Rescue");
    }



    [ContextMenu("ReduceOxygen")]
    public void ReduceOxygen(float currentOxygenValue)
    {
        NPCsOxygenLeft = (int)(currentOxygenValue / PlayerOxygen.MaxLevels * 100);
        NPCsOxygenDisplay.text = NPCsOxygenLeft.ToString() + "%";
        NPCsOxygenAnimator.SetBool("IsRunningOut", NPCsOxygenLeft <= ColorThresholdPercent);
        Vignette.SetBool("LowOxygen", NPCsOxygenLeft < ColorThresholdPercent);

        if (NPCsOxygenLeft > ColorThresholdPercent)
        {
            NPCsOxygenDisplay.color = OxygenOkColor;
        }
        else
        {
            NPCsOxygenDisplay.color = OxygenRunningOutColor;
        }
    }


    public void OnEndgame()
    {
        EndgamePopup.OnEndgame();
    }
}
