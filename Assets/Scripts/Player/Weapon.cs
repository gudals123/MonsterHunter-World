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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Attack");
        if (other.CompareTag("Boss"))
        {
            //타격 지점 계산
            Vector3 hitPos = other.ClosestPoint(transform.position);


            BattleManager.TakeDamage("Player", BattleManager._playerAttackDamege);
            Debug.Log(BattleManager._playerAttackDamege);

        }
    }
}
