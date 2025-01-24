using UnityEngine;



public class RescueShipCollisionDetector : MonoBehaviour
{
    [SerializeField] private RescueShip Ship;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            RescueNPCs();
        }
    }


    [ContextMenu("RescueNPCs")]
    public void RescueNPCs()
    {
        Ship.PlayerRescuedNPCs(Random.Range(0, 9));
    }
}
