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
    public static bool _isPlayerInRange;
    public static bool _isPlayerInBossView;

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
    /// �÷��̾ Boss�� ���� �ȿ� �ִ� �� Ȯ���ϴ� �޼ҵ� �Դϴ�.
    /// 1. player.position�� boss ���� ���̰�, Distance�� 9 ������ �� ���� ���� ���� üũ
    /// 2. player.position�� boss ���� ���̰�, Distance�� 18 ������ �� �þ� ���� ���� üũ
    /// </summary>
    /// <param name="player">�÷��̾��� ��ġ</param>
    /// <param name="boss">������ ��ġ</param>
    public static void isPlayerInRange(Transform player, Transform boss)
    {
        distancePtoB = Vector3.Distance(player.position, boss.position);

        Vector3 normalized = (player.position - boss.position).normalized;
        float _isForward = Vector3.Dot(normalized, boss.forward);

        if (_isForward > 0 && distancePtoB <= 9f)
        {
            _isPlayerInRange = true;
        }
        else _isPlayerInRange = false;

        if (_isForward > 0 && distancePtoB <= 18f) _isPlayerInBossView = true;
        else _isPlayerInBossView = false;
    }


    /// <summary>
    /// Player �Ǵ� Monster�� �޴� ������� ü�¿� �ݿ��ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">������� �ִ� ��ü�� �ǹ��մϴ�.</param>
    /// <param name="damage">�޴� ��������� �ǹ��մϴ�.</param>
    public static void TakeDamage(string type, float damage)
    {
        if (type == "Player")
            _currentBossHP -= damage;
        if (type == "Boss")
        {
            _currentPlayerHP -= damage;
            _bossGetHit = !_bossGetHit;
            _bossGetHit = !_bossGetHit;
        }
    }

    public void StartBreathAttack()
    {
        StartCoroutine(BreathAttack()); 
    }

    public static IEnumerator BreathAttack()
    {
        Debug.Log("Start Breath Attacking~~~~~~");
        // ���� �극�� ������Ʈ ��
        // CollisionEnter �� TakeDamage
        yield return new WaitForSeconds(2f);
        // ���� �극�� ������Ʈ ��
        Debug.Log("Quit Breath Attack~~~~~~");
    }
}