using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager instance;

    public static float distanceCanAttack = 1.5f;
    public static float distanceCanDetect = 4f;
    
    [Header("Boss Detect")]
    public static float distanceCatToBoss;
    public static bool _isBossInAttackRange;
    public static bool _isBossInCatView;

    [Header("Player Detect")]
    public static float distanceCatToPlayer;
    public static bool _isPlayerInAttackRange;
    public static bool _isPlayerInCatView;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void IsBossInRange(Transform boss, Transform cat)
    {
        distanceCatToBoss = Vector3.Distance(boss.position, cat.position);

        Vector3 normalized = (boss.position - cat.position).normalized;
        //float _isForward = Vector3.Dot(normalized, cat.forward);

        if (/*_isForward > 0 &&*/ distanceCatToBoss <= distanceCanAttack)
        {
            _isBossInAttackRange = true;
            Debug.Log($"StartAttackToBoss: {_isBossInAttackRange}");
        }
        else _isBossInAttackRange = false;

        if (/*_isForward > 0 &&*/ distanceCatToBoss <= distanceCanDetect)
        {
            _isBossInCatView = true;
            Debug.Log($"CanDetectBoss : {_isBossInCatView}");
        }
        else _isBossInCatView = false;
    }

    public static void IsPlayerInRange(Transform player, Transform cat)
    {
        distanceCatToPlayer = Vector3.Distance(player.position, cat.position);

        Vector3 normalized = (player.position - cat.position).normalized;
        //float _isForward = Vector3.Dot(normalized, cat.forward);

        if (/*_isForward > 0 &&*/ distanceCatToPlayer <= distanceCanAttack)
        {
            _isPlayerInAttackRange = true;
            Debug.Log($"FollowPlayer : {_isPlayerInAttackRange}");
        }
        else _isPlayerInAttackRange = false;

        if (/*_isForward > 0 &&*/ distanceCatToPlayer <= distanceCanDetect)
        {
            _isPlayerInCatView = true;
            Debug.Log($"CanDetectPlayer : {_isPlayerInCatView}");
            Debug.Log($"StartFollowPlayer : {_isPlayerInCatView}");
        }
        else _isPlayerInCatView = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceCanAttack);
        if (_isBossInAttackRange)
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(transform.position, distanceCanDetect);
        if (_isBossInCatView)
        {
            Gizmos.color = Color.blue;
        }
    }
}
