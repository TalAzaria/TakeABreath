using System;
using UnityEngine;

public class CreatureOxygen : MonoBehaviour
{
    public Action OnDepleted;
    public Action<float> OnChanged;
    private float changeRatePerSecondDefualt = -0.5f;
    public float ChangeRatePerSecond;
    public float MaxLevels = 50;
    private float levels;
    public float Levels
    {
        get { return levels; }
        set {
            if (value < 0)
            {
                levels = 0;
                OnDepleted?.Invoke();
            }
            else if (value > MaxLevels)
            {
                levels = MaxLevels;
            }
            else if (value != levels) 
            { 
                OnChanged?.Invoke(value);
                levels = value;
            }
        }
    }

    public void SetChangeRateToDefault()
    {
        ChangeRatePerSecond = changeRatePerSecondDefualt;
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
        Levels += ChangeRatePerSecond * Time.deltaTime;
    }
}
