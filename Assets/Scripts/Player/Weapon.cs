using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    /*    private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Attack");
            if (collision.gameObject.CompareTag("Boss"))
            {
                //추후 타격 지점에 타격 이펙트 생성 때 사용 예정
                //Vector3 pointOfContact = collision.contacts[0].point;

                BattleManager.TakeDamage("Player", BattleManager._playerAttackDamege);
                Debug.Log(BattleManager._playerAttackDamege);

            }

        }*/

    public GameObject _hitprefab;
    public GameObject _hitEffect;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Attack");
        if (other.CompareTag("Boss"))
        {
            //타격 지점 계산
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, other.gameObject);

            BattleManager.Instance.TakeDamage("Player", BattleManager.Instance._playerAttackDamege);
            Debug.Log(BattleManager.Instance._playerAttackDamege);

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
