using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class NPCVisual : MonoBehaviour
{
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
