using System;
using UnityEngine;

public class CreatureOxygen : MonoBehaviour
{
    public Action OnDepleted;
    private float changeRatePerSecondDefualt = -0.5f;
    public float ChangeRatePerSecond;
    public float MaxLevels = 50;
    private float levels;
    public float Levels
    {
        get { return levels; }
        set { 
            if(value < 0)
            {
                value = 0;
                OnDepleted?.Invoke();
            }
            else if(value > MaxLevels)
            {
                value = MaxLevels;
            }
            
            levels = value;
        }
    }

    public void SetChangeRateToDefault()
    {
        ChangeRatePerSecond = changeRatePerSecondDefualt;
    }

    private void Awake()
    {
        Levels = MaxLevels;
        SetChangeRateToDefault();
    }

    void Update()
    {
        Levels += ChangeRatePerSecond * Time.deltaTime;
    }
}
