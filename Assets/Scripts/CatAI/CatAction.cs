using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction : MonoBehaviour
{
    [SerializeField] private Rigidbody catRigidbody;

    // [Header("Distance")]
    public float distanceCanDetect { get; private set; } = 4f; // ���� ����
    public float distanceCanAttack { get; private set; } = 1.5f; // ���� ����

    // [Header("Player Detect")]
    public float distanceCatToPlayer { get; private set; }
    public Vector3 normalized { get; private set; }
    public bool _isPlayerInAttackRange { get; set; }
    public bool _isPlayerInCatView { get; set; }
    public bool _isPlayerAlmostDie { get; set; }
    [SerializeField] private Transform playerTransform;

    // [Header("Boss Detect")]
    public float distanceCatToBoss { get; private set; } // �Ÿ�
    public bool _isBossInAttackRange { get; set; } // ����
    public bool _isBossInCatView { get; set; } // ����
    [SerializeField] private Transform bossTransform;

    [Header("Cat Move")]
    private float rotationSpeed = 10;
    private float moveSpeed = 50;
    public float coolTime = 0;

    public void IsBossInRange(Transform boss, Transform cat)
    {
        distanceCatToBoss = Vector3.Distance(boss.position, cat.position);

        Vector3 normalized = (boss.position - cat.position).normalized;

        if (distanceCatToBoss <= distanceCanDetect && distanceCatToBoss >= distanceCanAttack)
        {
            _isBossInCatView = true;
            // Debug.Log($"CanDetectBoss : {_isBossInCatView}");
            cat.position += normalized / moveSpeed;
            LookAtTarget(bossTransform);
        }
        else _isBossInCatView = false;

        if (distanceCatToBoss <= distanceCanAttack)
        {
            _isBossInAttackRange = true;
            // Debug.Log($"StartAttackToBoss: {_isBossInAttackRange}");
        }
        else _isBossInAttackRange = false;
    }

    public void FollowPlayer(Transform player, Transform cat)
    {
        distanceCatToPlayer = Vector3.Distance(player.position, cat.position);

        normalized = (player.position - cat.position).normalized;

        if (!_isBossInCatView && !_isBossInAttackRange && distanceCatToPlayer >= distanceCanAttack)
        {
            _isPlayerInAttackRange = false;
            // Debug.Log($"StartFollowPlayer : {_isPlayerInAttackRange}");
            cat.position += normalized / moveSpeed;
            LookAtTarget(playerTransform);
        }
        else _isPlayerInAttackRange = true;

        if (distanceCatToPlayer <= distanceCanAttack)
        {
            _isPlayerInCatView = false;
            // Debug.Log($"CanDetectPlayer : {_isPlayerInCatView}");
            // Debug.Log($"FollowPlayer : {_isPlayerInCatView}");
        }
        else _isPlayerInCatView = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceCanAttack);
        Gizmos.DrawWireSphere(transform.position, distanceCanDetect);
    }

    public void LookAtTarget(Transform target)
    {
        Vector3 dir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(catRigidbody.transform.position.x, 0, catRigidbody.transform.position.z);
        catRigidbody.transform.rotation = Quaternion.Lerp(catRigidbody.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
    }

    public bool PlayerHPCheck()
    {
        // Debug.Log($"playerHP : {CombatManager.Instance._currentPlayerHP}");
        // Debug.Log($"coolTime : {coolTime}");
        if (CombatManager.Instance._currentPlayerHP <= 40)
        {
            coolTime += Time.deltaTime;
            return true;
        }
        else
        {
            coolTime = 0;
            return false;
        }
    }

    public void Heal()
    {
        if (coolTime >= 5)
        {
            // Debug.Log("Heal");
            CombatManager.Instance._currentPlayerHP += 20;
            coolTime = 0;
        }
    }
}