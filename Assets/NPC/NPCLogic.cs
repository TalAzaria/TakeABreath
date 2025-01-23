using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    private Transform playerTransform;
    public Vector2 baseOffset = new Vector2(1.0f, 0);
    private float spacing = 0.6f;
    [SerializeField] private int numOfPeopleCollected;

    private void Start()
    {
        playerTransform = this.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            collision.transform.SetParent(playerTransform);

            int attachedCount = playerTransform.childCount - 1;
            Vector2 offset = new Vector2(baseOffset.x + (attachedCount * spacing), baseOffset.y);
            collision.transform.localPosition = offset;

            numOfPeopleCollected++;

            this.GetComponent<PlayerMovement>().OnIncrementCollectedPeopleDown();
        }
    }
}