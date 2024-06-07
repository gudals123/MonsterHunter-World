using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject _hitprefab;
    public GameObject _hitEffect;

    private float attackDamege;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            //타격 지점 계산
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, other.gameObject);

            attackDamege = CombatManager.Instance.PlayerAttackDamegeCalculation();
            CombatManager.Instance.TakeDamage("Player", attackDamege);
            //Debug.Log(attackDamege);

        }
    }

    public void AppearHitEffect(Vector3 hitPos, GameObject player)
    {
        StartCoroutine(CoHitEffect(hitPos, player));
    }

    public IEnumerator CoHitEffect(Vector3 hitPos, GameObject player)
    {
        if (_hitEffect == null)
        {
            _hitprefab = Resources.Load<GameObject>("Prefabs/HitEffectSample 1");
            _hitEffect = Instantiate(_hitprefab);
            _hitEffect.transform.parent = player.transform;
        }

        _hitEffect.transform.position = hitPos;
        _hitEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        _hitEffect.SetActive(false);
    }

}
