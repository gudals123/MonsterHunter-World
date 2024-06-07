using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager instance;

    [SerializeField] public int _bossMaxHP = 2000;
    [SerializeField] public int _playerMaxHP = 100;

    public float _currentBossHP { get; set; }
    public float _currentPlayerHP;

    public bool _isPlayerDead = false;
    public bool _isBossDead { get; set; } = false;
    public bool _isBossRecognizedPlayer;
    public bool _isCharging = false;

    public float distancePtoB;
    public bool _bossAttackRange;
    public bool _bossAttackBackRange;
    public bool _bossVisualRange;
    public bool _bossPerceptionRange;

    public float _playerAttackDamege;


    public static CombatManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject();
                instance = singletonObject.AddComponent<CombatManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Initialize();
    }


    private void Initialize()
    {
        _currentBossHP = _bossMaxHP;
        _currentPlayerHP = _playerMaxHP;

        _isBossDead = false;
        _isPlayerDead = false;
    }




    /// <summary>
    /// �÷��̾ Boss�� ���� �ȿ� �ִ� �� Ȯ���ϴ� �޼ҵ� �Դϴ�.
    /// 1. boss.position�� player ���� ���̰�, Distance�� 9 ������ �� ���� ���� ���� üũ
    /// 2. boss.position�� player ���� ���̰�, Distance�� 18 ������ �� �þ� ���� ���� üũ
    /// </summary>
    /// <param name="player">�÷��̾��� ��ġ</param>
    /// <param name="boss">������ ��ġ</param>
    public void isPlayerInRange(Transform player, Transform boss)
    {
        distancePtoB = Vector3.Distance(player.position, boss.position);

        Vector3 normalized = (player.position - boss.position).normalized;
        float _isForward = Vector3.Dot(normalized, boss.forward);

        // ���� ����
        if (_isForward > 0 && distancePtoB <= 9f)
        {
            _bossAttackRange = true;
            _isBossRecognizedPlayer = true;
        }
        else
        {
            _bossAttackRange = false;
            _bossAttackBackRange = false;
        }

        // �þ� ����
        if (_isForward > 0 && distancePtoB <= 18f)
        {
            _bossVisualRange = true;
            _isBossRecognizedPlayer = true;
        }
        else 
        {
            _bossVisualRange = false;
            _isBossRecognizedPlayer = false;
        }

        //�ν� ����
        if (distancePtoB <= 18f) _bossPerceptionRange = true;
        else _bossPerceptionRange = false;
    }


    /// <summary>
    /// Player �Ǵ� Monster�� �޴� �������? ü�¿� �ݿ��ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">�������? �ִ� ��ü�� �ǹ��մϴ�.</param>
    /// <param name="damage">�޴� ���������? �ǹ��մϴ�.</param>
    public void TakeDamage(string type, float damage)
    {
        if (type == "Player")
            _currentBossHP -= damage;
            _isBossRecognizedPlayer = true;
        if (type == "Boss")
        {
            _currentPlayerHP -= damage;
        }
    }

}
