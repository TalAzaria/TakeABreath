using UnityEngine;
using TMPro;



public class RescueShip : MonoBehaviour
{
    [SerializeField] Animator RescuedAnimator;
    [SerializeField] TextMeshPro RescuedDisplay;
    [SerializeField] NPCLogic Player;
    [SerializeField] AudioSource MyAudioSource;


    private void Start()
    {
        Player.OnReachSurfaceWithNpc += PlayerRescuedNPCs;
    }


    public void PlayerRescuedNPCs(int numberOfNPCs)
    {
        RescuedDisplay.text = "+" + numberOfNPCs.ToString();
        RescuedAnimator.SetTrigger("Rescue");
        MainUI.Instance.PlayerRescuedNPCs(numberOfNPCs);
        MyAudioSource.Play();
    }
}
