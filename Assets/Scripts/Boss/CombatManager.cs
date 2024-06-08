using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager instance = null;

    [SerializeField] public float _bossMaxHP = 2000;
    [SerializeField] public float _playerMaxHP = 100;
    public float _chargingStartTime{ get; private set; } = 0f;
    private float _chargingEndTime = 0f;
    

    public float _currentBossHP { get; private set; }
    public float _currentPlayerHP{ get; private set; }

    public bool _isPlayerDead{ get; private set; }
    public bool _isBossDead { get; private set; }
    public bool _isBossRecognizedPlayer{ get; set; }
    public bool _isRightAttak { get; set; }
    private bool _isChargingField;
    public bool _isCharging
    { 
        get
        {
            return _isChargingField;
        }
        set 
        {
            if(value == true)
            {
                _chargingStartTime = Time.time;
            }
            else
            {
                _chargingEndTime = Time.time;
            }
            _isChargingField = value;
        }
    }

    public float distancePtoB{ get; private set; }
    public bool _bossAttackRange{ get; private set; }
    public bool _bossAttackBackRange{ get; private set; }
    public bool _bossVisualRange{ get; private set; }
    public bool _bossPerceptionRange{ get; private set; }
    public float _playerAttackDamege{get; private set;}


    private void Awake()
    {
        // ?¸?Š¤?„´?Š¤ê°? null?´ë©? ?˜„?¬ ?¸?Š¤?„´?Š¤ë¥? ?• ?‹¹
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ?”¬?´ ë°”ë?Œì–´?„ ?ŒŒê´´ë˜ì§? ?•Š?„ë¡? ?„¤? •
            Initialize();
        }
        // ?¸?Š¤?„´?Š¤ê°? ?´ë¯? ì¡´ì¬?•˜ê³?, ?˜„?¬ ?¸?Š¤?„´?Š¤ê°? ê·? ?¸?Š¤?„´?Š¤??? ?‹¤ë¥´ë©´ ??‹ ?„ ?ŒŒê´?
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

    private float ChargingTimeCalculation()
    {
        return _chargingEndTime - _chargingStartTime;
    }

    public float PlayerAttackDamegeCalculation()
    {
        float chargingTime = ChargingTimeCalculation();
        
        //Debug.Log(chargingTime);

        if(_isRightAttak)
        {
            _playerAttackDamege = 20f;
        }
        else
        {
            if(chargingTime < 0.5)
            {
                _playerAttackDamege = 15f;
            }
            else if(chargingTime < 1.5)
            {
                _playerAttackDamege = 30f;
            }
            else
            {
                _playerAttackDamege = 60f;
            }
        }
        return _playerAttackDamege;
    }

    /// <summary>
    /// ï¿½Ã·ï¿½ï¿½Ì¾î°¡ Bossï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½È¿ï¿½ ï¿½Ö´ï¿½ ï¿½ï¿½ È®ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½Ş¼Òµï¿½ ï¿½Ô´Ï´ï¿½.
    /// 1. boss.positionï¿½ï¿½ player ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Ì°ï¿½, Distanceï¿½ï¿½ 9 ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ Ã¼Å©
    /// 2. boss.positionï¿½ï¿½ player ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Ì°ï¿½, Distanceï¿½ï¿½ 18 ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½Ã¾ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ Ã¼Å©
    /// </summary>
    /// <param name="player">ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡</param>
    /// <param name="boss">ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡</param>
    public void isPlayerInRange(Transform player, Transform boss)
    {
        distancePtoB = Vector3.Distance(player.position, boss.position);

        Vector3 normalized = (player.position - boss.position).normalized;
        float _isForward = Vector3.Dot(normalized, boss.forward);

        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
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

        // ï¿½Ã¾ï¿½ ï¿½ï¿½ï¿½ï¿½
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

        //ï¿½Î½ï¿½ ï¿½ï¿½ï¿½ï¿½
        if (distancePtoB <= 18f) _bossPerceptionRange = true;
        else _bossPerceptionRange = false;
    }


    /// <summary>
    /// Player ï¿½Ç´ï¿½ Monsterï¿½ï¿½ ï¿½Ş´ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿?? Ã¼ï¿½Â¿ï¿½ ï¿½İ¿ï¿½ï¿½Ï´ï¿½ ï¿½Ş¼Òµï¿½ï¿½Ô´Ï´ï¿½.
    /// </summary>
    /// <param name="type">ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿?? ï¿½Ö´ï¿½ ï¿½ï¿½Ã¼ï¿½ï¿½ ï¿½Ç¹ï¿½ï¿½Õ´Ï´ï¿½.</param>
    /// <param name="damage">ï¿½Ş´ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿?? ï¿½Ç¹ï¿½ï¿½Õ´Ï´ï¿½.</param>
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
            if (_currentPlayerHP <= 0)
            {
                _isPlayerDead = true;
            }
        }
    }

}
