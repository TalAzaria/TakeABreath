using UnityEngine;



public class BubbleEmitter : MonoBehaviour
{
    [SerializeField] private ParticleSystem Particles;
    [SerializeField] private ParticleSystem.TriggerModule ParticleTriggerModule;

    [SerializeField] private bool isMainMenu = false;


    private void Start()
    {
        if (!isMainMenu)
        {
            Surface surface = FindFirstObjectByType<Surface>();
            Collider collider = surface.GetComponent<Collider>();
            ParticleTriggerModule = Particles.trigger;
            ParticleTriggerModule.SetCollider(0, collider);
        }

    }
}
