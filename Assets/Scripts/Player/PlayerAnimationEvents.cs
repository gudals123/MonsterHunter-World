using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject _handWeapon;
    [SerializeField] private GameObject _BackWeapon;

    [SerializeField] private GameObject _attackRange;


    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();   
        _handWeapon.SetActive(false);
        _BackWeapon.SetActive(true);
        _attackRange.SetActive(false);
    }

    public void DrawWeapon()
    {
        _handWeapon.SetActive(true);
        _BackWeapon.SetActive(false);
    }

    public void SheatheWeapon()
    {
        _handWeapon.SetActive(false);
        _BackWeapon.SetActive(true);
    }

    public void AnimationPause()
    {
        GetComponent<Animator>().speed = 0.01f;
        CombatManager.Instance._isCharging = true;
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
