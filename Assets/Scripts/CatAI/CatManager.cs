using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    private static CatManager instance = null;

    [SerializeField] private Rigidbody catRigidbody;

    // [Header("Distance")]
    public static float distanceCanDetect { get; private set; } = 4f; // ���� ����
    public static float distanceCanAttack { get; private set; } = 1.5f; // ���� ����

    // [Header("Player Detect")]
    public static float distanceCatToPlayer { get; private set; }
    public static Vector3 normalized { get; private set; }
    public static bool _isPlayerInAttackRange { get; set; }
    public static bool _isPlayerInCatView { get; set; }
    public static bool _isPlayerAlmostDie { get; set; }
    [SerializeField] private Transform player;

    // [Header("Boss Detect")]
    public static float distanceCatToBoss { get; private set; } // �Ÿ�
    public static bool _isBossInAttackRange { get; set; } // ����
    public static bool _isBossInCatView { get; set; } // ����
    [SerializeField] private Transform boss;

    private float rotationSpeed = 10;
    private float moveSpeed = 50;

    public float coolTime = 0;

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

    public static CatManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    public void IsBossInRange(Transform boss, Transform cat)
    {
        distanceCatToBoss = Vector3.Distance(boss.position, cat.position);

        Vector3 normalized = (boss.position - cat.position).normalized;

        if (distanceCatToBoss <= distanceCanDetect && distanceCatToBoss >= distanceCanAttack)
        {
            _isBossInCatView = true;
            // Debug.Log($"CanDetectBoss : {_isBossInCatView}");
            cat.position += normalized / moveSpeed;
            LookAtTarget(boss);
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
            LookAtTarget(player);
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
        Vector3 dir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(catRigidbody.transform.position.x, 0, catRigidbody.transform.position.z);
        catRigidbody.transform.rotation = Quaternion.Lerp(catRigidbody.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
    }

    // public static bool PlayerHPCheck()
    // {
    //     Debug.Log($"playerHP : {CombatManager.Instance._currentPlayerHP}");
    //     if (CombatManager.Instance._currentPlayerHP <= 30)
    //     {
    //         _isPlayerAlmostDie = true;
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }

    // public static void Heal()
    // {
    //     Debug.Log("Heal");
    //     CombatManager.Instance._currentPlayerHP += 30;
    // }

    // public static bool PlayerHPCheck()
    // {
    //     Debug.Log($"playerHP : {Instance._currentPlayerHP}");
    //     if (Instance._currentPlayerHP <= 30)
    //     {
    //         CatManager._isPlayerAlmostDie = true;
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }

    // public static void Heal()
    // {
    //     if(coolTime >= 5)
    //     {
    //         Debug.Log("Heal");
    //         CombatManager.Instance._currentPlayerHP += 30;
    //     }
    //     coolTime = 0;
    // }
}