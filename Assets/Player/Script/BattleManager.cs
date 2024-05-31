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
    [SerializeField] public static int _PlayerMaxHP = 100;

    public int _currentPlayerHP;

    public bool _isPlayerDead = false;    


    void Awake()
    {
        _currentPlayerHP = _PlayerMaxHP;
        _isPlayerDead = false;

    }


    void Update()
    {
        
    }
}
