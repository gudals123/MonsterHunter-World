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
                //���� Ÿ�� ������ Ÿ�� ����Ʈ ���� �� ��� ����
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
            //Ÿ�� ���� ���
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, other.gameObject);

            CombatManager.Instance.TakeDamage("Player", CombatManager.Instance._playerAttackDamege);
            Debug.Log(CombatManager.Instance._playerAttackDamege);

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
