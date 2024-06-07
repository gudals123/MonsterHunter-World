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
            // 인스턴스가 null이면 새로 생성
            if (instance == null)
            {
                instance = FindObjectOfType<CombatManager>();

                // 씬에 존재하지 않는 경우 새로운 게임 오브젝트를 생성하여 추가
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("CombatManagerSingleton");
                    instance = singletonObject.AddComponent<CombatManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

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
