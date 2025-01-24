using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum NPCTypes
{
    Cat1 = 0,
    Cat2 = 1,
    Cat3 = 2,
    Dog1 = 3,
    Dog2 = 4,
    Dog3 = 5,
    Duck1 = 6,
    Duck2 = 7
}



public class NPCVisual : MonoBehaviour
{
    public NPCTypes NPCType = NPCTypes.Cat1;
    [SerializeField] private SpriteRenderer SpriteHolder;
    [SerializeField] private Sprite AliveSprite1;
    [SerializeField] private Sprite AliveSprite2;
    [SerializeField] private Sprite DeadSprite;
    [SerializeField] private float SwapInterval = 0.25f;


    bool isAlive = true;
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            SpriteHolder.sprite = isAlive ? AliveSprite1 : DeadSprite;
        }
    }


    private void Start()
    {
        StartCoroutine(SwapSprites());
    }



    IEnumerator SwapSprites()
    {
        yield return new WaitForSeconds(SwapInterval);

        if (IsAlive)
        {
            SpriteHolder.sprite = (SpriteHolder.sprite == AliveSprite1) ? AliveSprite2 : AliveSprite1;
        }
        else
        {
            SpriteHolder.sprite = DeadSprite;
        }

        StartCoroutine(SwapSprites());
    }
}
