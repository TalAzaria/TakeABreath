using UnityEngine;
using UnityEngine.UI;

public class NpcHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private CreatureOxygen creatureOxygen;
    void Start()
    {
        // Find the CreatureOxygen component on the NPC (if not assigned directly)

        if (creatureOxygen != null)
        {
            // Subscribe to the OnChanged event
            creatureOxygen.OnChanged += UpdateOxygenSlider;

            // Initialize the slider with current levels
            InitializeOxygenSlider();
        }
        else
        {
            Debug.LogError("CreatureOxygen script is missing on the GameObject.");
        }
    }
    private void InitializeOxygenSlider()
    {
        if (slider != null)
        {
            slider.maxValue = creatureOxygen.MaxLevels;
            slider.value = creatureOxygen.Levels;
        }
    }

    private void UpdateOxygenSlider(float levels)
    {
        if (slider != null)
        {
            slider.value = levels;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (creatureOxygen != null)
        {
            creatureOxygen.OnChanged -= UpdateOxygenSlider;
        }
    }
    void Update()
    {
        
    }
}
