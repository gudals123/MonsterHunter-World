using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager instance;
    [SerializeField] private static Rigidbody catRigidbody;

    public static float distanceCanDetect = 4f; // 감지 범위
    public static float distanceCanAttack = 1.5f; // 공격 범위

    [Header("Boss Detect")]
    public static float distanceCatToBoss; // 거리
    public static bool _isBossInAttackRange; // 공격
    public static bool _isBossInCatView; // 감지
    [SerializeField] private Transform boss;

    [Header("Player Detect")]
    public static float distanceCatToPlayer;
    public static bool _isPlayerInAttackRange;
    public static bool _isPlayerInCatView;
    [SerializeField] private Transform player;

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

        catRigidbody = GetComponentInChildren<Rigidbody>();
    }

    public static void IsBossInRange(Transform boss, Transform cat)
    {
        distanceCatToBoss = Vector3.Distance(boss.position, cat.position);

        Vector3 normalized = (boss.position - cat.position).normalized;
        //float _isForward = Vector3.Dot(normalized, cat.forward);

        if (/*_isForward > 0 &&*/ distanceCatToBoss <= distanceCanDetect && distanceCatToBoss >= distanceCanAttack)
        {
            _isBossInCatView = true;
            Debug.Log($"CanDetectBoss : {_isBossInCatView}");
            cat.position += normalized / 100;
            LookAtTarget(boss);
        }
        else _isBossInCatView = false;

        if (/*_isForward > 0 &&*/ distanceCatToBoss <= distanceCanAttack)
        {
            _isBossInAttackRange = true;
            Debug.Log($"StartAttackToBoss: {_isBossInAttackRange}");
        }
        else _isBossInAttackRange = false;
    }

    public static void IsPlayerInRange(Transform player, Transform cat)
    {
        distanceCatToPlayer = Vector3.Distance(player.position, cat.position);

        Vector3 normalized = (player.position - cat.position).normalized;
        //float _isForward = Vector3.Dot(normalized, cat.forward);

        if (/*_isForward > 0 &&*/ distanceCatToPlayer <= distanceCanDetect && distanceCatToPlayer >= distanceCanAttack)
        {
            _isPlayerInAttackRange = true;
            Debug.Log($"StartFollowPlayer : {_isPlayerInAttackRange}");
            cat.position += normalized / 100;
            LookAtTarget(player);
        }
        else _isPlayerInAttackRange = false;

        if (/*_isForward > 0 &&*/ distanceCatToPlayer <= distanceCanAttack)
        {
            _isPlayerInCatView = true;
            Debug.Log($"CanDetectPlayer : {_isPlayerInCatView}");
            Debug.Log($"FollowPlayer : {_isPlayerInCatView}");
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
        else
        {
            Gizmos.color = Color.white;
        }

        Gizmos.DrawWireSphere(transform.position, distanceCanDetect);
        if (_isBossInCatView)
        {
            Gizmos.color = Color.blue;
        }
    }

    private static void LookAtTarget(Transform target)
    {
        Vector3 dir = target.transform.position - catRigidbody.transform.position;
        catRigidbody.transform.rotation = Quaternion.Lerp(catRigidbody.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
    }

    //public void LookAtPlayer()
    //{
    //    transform.LookAt(player);
    //}

    //public void LookAtBoss()
    //{
    //    transform.LookAt(boss);
    //}
}
