using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;

    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject();
                instance = singletonObject.AddComponent<BattleManager>();
            }
            return instance;
        }
    }


    [Header("Status")]
    [SerializeField] public static float _PlayerMaxHP = 100;
    [SerializeField] public static float _BossMaxHP = 1000;

    public float _currentPlayerHP;
    public float _currentBossHP;

    public bool _isPlayerDead = false;
    public bool _isCharging = false;

    public float _playerAttackDamege;

    void Awake()
    {
        _currentPlayerHP = _PlayerMaxHP;
        _currentBossHP = _BossMaxHP;
        _isPlayerDead = false;

    }

    public void TakeDamage(string type, float damage)
    {
        if (type == "Player")
        {
            _currentBossHP -= damage;
        }

    }
}
