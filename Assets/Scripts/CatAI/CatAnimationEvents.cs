using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationEvents : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject _handWeapon;
    [SerializeField] private GameObject _BackWeapon;
    [SerializeField] private GameObject _attackRange;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();   
        _handWeapon.SetActive(true);
        _BackWeapon.SetActive(false);
        _attackRange.SetActive(false);
    }

    public void AttackRangeCollierTurnOn()
    {
        _attackRange.SetActive(true);
    }

    public void AttackRangeCollierTurnOff()
    {
        _attackRange.SetActive(false);
    }
}
