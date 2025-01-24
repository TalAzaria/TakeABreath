using System;
using UnityEngine;

public class CreatureOxygen : MonoBehaviour
{
    public Action<GameObject> OnDepleted;
    public GameOverManager gameOverManager;
    public Action<float> OnChanged;
    private float changeRatePerSecondDefualt = -0.5f;
    public float changeRatePerSecond;
    public float MaxLevels = 50;
    [SerializeField] private float levels;
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
                    gameOverManager?.EndGame();
                    gameOverManager = null;
                }
            }
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

    private void Awake()
    {
        Levels = MaxLevels;
        SetChangeRateToDefault();
    }

    void Update()
    {
        Levels += changeRatePerSecond * Time.deltaTime;
    }
}