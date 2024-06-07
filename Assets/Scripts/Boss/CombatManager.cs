using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager instance = null;

    [SerializeField] public int _bossMaxHP = 2000;
    [SerializeField] public int _playerMaxHP = 100;

    public float _currentBossHP { get; private set; }
    public float _currentPlayerHP{ get; private set; }

    public bool _isPlayerDead{ get; private set; }
    public bool _isBossDead { get; private set; }
    public bool _isBossRecognizedPlayer{ get; set; }
    public bool _isCharging{ get; set; }

    public float distancePtoB{ get; private set; }
    public bool _bossAttackRange{ get; private set; }
    public bool _bossAttackBackRange{ get; private set; }
    public bool _bossVisualRange{ get; private set; }
    public bool _bossPerceptionRange{ get; private set; }

    public float _playerAttackDamege{ get; private set; }

    private void Awake()
    {
        // 인스턴스가 null이면 현재 인스턴스를 할당
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
            Initialize();
        }
        // 인스턴스가 이미 존재하고, 현재 인스턴스가 그 인스턴스와 다르면 자신을 파괴
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static CombatManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    
    private void Initialize()
    {
        _currentBossHP = _bossMaxHP;
        _currentPlayerHP = _playerMaxHP;

        _isBossDead = false;
        _isPlayerDead = false;
        _isCharging = false;
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
        {
            _currentBossHP -= damage;
            _isBossRecognizedPlayer = true;
            if (_currentBossHP <= 0)
            {
                _isBossDead = true;
            }
        }
        if (type == "Boss")
        {
            _currentPlayerHP -= damage;
        }
    }

}
