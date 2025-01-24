using UnityEngine;
using TMPro;



public class RescueShip : MonoBehaviour
{
    [SerializeField] Animator RescuedAnimator;
    [SerializeField] TextMeshPro RescuedDisplay;


    public void PlayerRescuedNPCs(int numberOfNPCs)
    {
        RescuedDisplay.text = "+" + numberOfNPCs.ToString();
        RescuedAnimator.SetTrigger("Rescue");
        MainUI.Instance.PlayerRescuedNPCs(numberOfNPCs);
    }
}
