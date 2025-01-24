using UnityEngine;



public class BubbleEmitter : MonoBehaviour
{
    [SerializeField] private ParticleSystem Particles;
    [SerializeField] private ParticleSystem.TriggerModule ParticleTriggerModule;


    private void Start()
    {
        Surface surface = FindFirstObjectByType<Surface>();
        Collider collider = surface.GetComponent<Collider>();
        ParticleTriggerModule = Particles.trigger;
        ParticleTriggerModule.SetCollider(0, collider);
    }
}
