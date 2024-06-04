using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager _instance;

    [SerializeField] public static int _bossMaxHP = 2000;
    [SerializeField] public static int _playerMaxHP = 100;

    public static float _currentBossHP { get; set; }
    public static float _currentPlayerHP;

    public static float _attackRange = 3.5f;
    public static bool _isPlayerDead = false;
    public static bool _isBossDead { get; set; } = false;

    public static float distancePtoB;
    public static bool _bossAttackRange;
    public static bool _bossAttackBackRange;
    public static bool _bossVisualRange;
    public static bool _bossPerceptionRange;
    public static bool _isbossRecognizedPlayer;

    public static bool _bossGetHit;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void Initialize()
    {
        _currentBossHP = _bossMaxHP;
        _currentPlayerHP = _playerMaxHP;

        _isBossDead = false;
        _isPlayerDead = false;
    }


    /// <summary>
    /// 플레이어가 Boss의 범위 안에 있는 지 확인하는 메소드 입니다.
    /// 1. boss.position이 player 보다 앞이고, Distance가 9 이하일 때 공격 범위 여부 체크
    /// 2. boss.position이 player 보다 앞이고, Distance가 18 이하일 때 시야 범위 여부 체크
    /// </summary>
    /// <param name="player">플레이어의 위치</param>
    /// <param name="boss">보스의 위치</param>
    public static void isPlayerInRange(Transform player, Transform boss)
    {
        distancePtoB = Vector3.Distance(player.position, boss.position);

        Vector3 normalized = (player.position - boss.position).normalized;
        float _isForward = Vector3.Dot(normalized, boss.forward);

        // 공격 범위
        if (_isForward > 0 && distancePtoB <= 9f)
        {
            _bossAttackRange = true;
            _bossAttackBackRange = false;
            _isbossRecognizedPlayer = true;
        }
        else if (_isForward < 0 && distancePtoB <= 9f)
        {
            _bossAttackRange = false;
            _bossAttackBackRange = true;
            _isbossRecognizedPlayer = true;
        }
        else
        {
            _bossAttackRange = false;
            _bossAttackBackRange = false;
        }

        // 시야 범위
        if (_isForward > 0 && distancePtoB <= 18f) _bossVisualRange = true;
        else _bossVisualRange = false;

        //인식 범위
        if (distancePtoB <= 18f) _bossPerceptionRange = true;
        else _bossPerceptionRange = false;
    }


    /// <summary>
    /// Player 또는 Monster가 받는 대미지를 체력에 반영하는 메소드입니다.
    /// </summary>
    /// <param name="type">대미지를 주는 주체를 의미합니다.</param>
    /// <param name="damage">받는 대미지량을 의미합니다.</param>
    public static void TakeDamage(string type, float damage)
    {
        if (type == "Player")
            _currentBossHP -= damage;
            _isbossRecognizedPlayer = true;
        if (type == "Boss")
        {
            _currentPlayerHP -= damage;
            _bossGetHit = true;
            _bossGetHit = false;
        }
    }


    //////////////////////////////////////////////////////////////////////////
    public void StartBreathAttack()
    {
        StartCoroutine(BreathAttack()); 
    }

    public static IEnumerator BreathAttack()
    {
        Debug.Log("Start Breath Attacking~~~~~~");
        // 직선 브레스 오브젝트 켬
        // CollisionEnter 시 TakeDamage
        yield return new WaitForSeconds(2f);
        // 직선 브레스 오브젝트 끔
        Debug.Log("Quit Breath Attack~~~~~~");
    }
}
