using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager instance;
    
    [SerializeField] private Rigidbody catRigidbody;

    [Header("Distance")]
    public static float distanceCanDetect = 4f; // ���� ����
    public static float distanceCanAttack = 1.5f; // ���� ����

    [Header("Player Detect")]
    public static float distanceCatToPlayer;
    public static Vector3 normalized;
    public static bool _isPlayerInAttackRange;
    public static bool _isPlayerInCatView;
    [SerializeField] private Transform player;
    
    [Header("Boss Detect")]
    public static float distanceCatToBoss; // �Ÿ�
    public static bool _isBossInAttackRange; // ����
    public static bool _isBossInCatView; // ����
    [SerializeField] private Transform boss;

    private float rotationSpeed = 10;
    private float moveSpeed = 50;

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

    public void IsBossInRange(Transform boss, Transform cat)
    {
        distanceCatToBoss = Vector3.Distance(boss.position, cat.position);

        Vector3 normalized = (boss.position - cat.position).normalized;

        if (distanceCatToBoss <= distanceCanDetect && distanceCatToBoss >= distanceCanAttack)
        {
            _isBossInCatView = true;
            Debug.Log($"CanDetectBoss : {_isBossInCatView}");
            cat.position += normalized / moveSpeed;
            LookAtTarget(boss);
        }
        else _isBossInCatView = false;

        if (distanceCatToBoss <= distanceCanAttack)
        {
            _isBossInAttackRange = true;
            Debug.Log($"StartAttackToBoss: {_isBossInAttackRange}");
        }
        else _isBossInAttackRange = false;

    }

    public void FollowPlayer(Transform player, Transform cat)
    {
        distanceCatToPlayer = Vector3.Distance(player.position, cat.position);

        normalized = (player.position - cat.position).normalized;

        if (!_isBossInCatView && !_isBossInAttackRange && distanceCatToPlayer >= distanceCanAttack)
        {
            _isPlayerInAttackRange = true;
            Debug.Log($"StartFollowPlayer : {_isPlayerInAttackRange}");
            cat.position += normalized / moveSpeed;
            LookAtTarget(player);
        }
        else _isPlayerInAttackRange = false;

        if (distanceCatToPlayer <= distanceCanAttack)
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

    public void LookAtTarget(Transform target)
    {
        Vector3 dir = new Vector3 (target.transform.position.x, 0, target.transform.position.z) - new Vector3 (catRigidbody.transform.position.x, 0, catRigidbody.transform.position.z);
        catRigidbody.transform.rotation = Quaternion.Lerp(catRigidbody.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
    }
}
