using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatureOxygen : MonoBehaviour
{
    public ParticleSystem bubblesParticles;
    public Action<GameObject> OnDepleted;
    public GameOverManager gameOverManager;
    public Action<float> OnChanged;
    private float changeRatePerSecondDefualt = -0.5f;
    public float changeRatePerSecond;
    public float MaxLevels = 50;
    [SerializeField] private float levels;
    public bool isInsideBubble = false;
    public bool isHoldingOnNpc = false;
    public NPCLogic npcLogic;

    public bool isPlayer = false;
    public float Levels
    {
        get { return levels; }
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > MaxLevels)
            {
                value = MaxLevels;
            }

            if (value != levels)
            {
                OnChanged?.Invoke(value);
                levels = value;
                if(value == 0)
                {
                    OnDepleted?.Invoke(this.gameObject);
                    OnDepleted = null;
                }
            }
        }
    }

    private void Awake()
    {
        Levels = MaxLevels;
    }

    private void Start()
    {
        isHoldingOnNpc = false;
        npcLogic = NPCLogic.Instance;

        if (!isPlayer)
        {
            npcLogic.InsideBubble(this.gameObject);
        }
    }

    public void SetChangeRateToDefault()
    {
        changeRatePerSecond = changeRatePerSecondDefualt;
    }

    public void SetLevelsToMax()
    {
        Levels = MaxLevels;
    }

    void Update()
    {
        Levels += changeRatePerSecond * Time.deltaTime;

        if (this.isInsideBubble && !isPlayer)
        {
            if (this.gameObject != null)
            {
                npcLogic.InsideBubble(this.gameObject);
            }
        }
    }
}