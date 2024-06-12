using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject _hitEffect;
    protected GameObject hit;
    protected float duration;
    protected int attackDamage;

    protected virtual void Awake()
    {
        hit = Instantiate(_hitEffect);
        hit.SetActive(false);
    }


    protected virtual void OnTriggerEnter(Collider other) { }

    protected virtual void AppearHitEffect(Vector3 hitPos, float duration)
    {
        StartCoroutine(CoHitEffect(hitPos, duration));
    }

    protected virtual IEnumerator CoHitEffect(Vector3 hitPos, float duration)
    {
        hit.transform.position = hitPos;
        hit.SetActive(true);
        yield return new WaitForSeconds(duration);
        hit.SetActive(false);
    }

}