using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject _attackRange;


    private PlayerController _playerController;
    private Player _player;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _player  =  GetComponentInParent<Player>();
        _attackRange.SetActive(false);
    }

    public void DrawWeapon()
    {
        _player.WeaponSetActive();
    }

    public void SheatheWeapon()
    {
        _player.WeaponSetActive();
    }

    public void AnimationPause()
    {
        if(_playerController.isInputAttack)
        {
            GetComponent<Animator>().speed = 0.01f;
        }
        _playerController.isCharging = true;
        _playerController.isAnimationPauseDone = true;
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
