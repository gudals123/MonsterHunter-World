using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject _hitEffect;
    public ParticleSystem hitParticleEffect;
    protected GameObject hit;

    public int attackDamage { get; set; }

    protected virtual void Awake()
    {
        hit = Instantiate(_hitEffect);
        hitParticleEffect = hit.GetComponent<ParticleSystem>();
    }

    protected virtual void OnTriggerEnter(Collider other) { }

    protected virtual void AppearHitEffect(Vector3 hitPos)
    {
        hit.transform.position = hitPos;
        hitParticleEffect.Play();
    }
}