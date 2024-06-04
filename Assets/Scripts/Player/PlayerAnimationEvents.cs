using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject _handWeapon;
    [SerializeField] private GameObject _BackWeapon;

    [SerializeField] private Transform _rangeOfAttack;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();   
        _handWeapon.SetActive(false);
        _BackWeapon.SetActive(true);
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
        GetComponent<Animator>().speed = 0.03f;
    }

    private void AttackTrigger()
    {
        Collider[] colliders = Physics.OverlapBox(_rangeOfAttack.position, new Vector3(1, 2, 1.5f));
            //.OverlapSphere(_rangeOfAttack.position, 1f);
            //OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Boss"))
            {
                BattleManager.TakeDamage("Player", BattleManager._playerAttackDamege);
                Debug.Log($"Attack Damege : {BattleManager._playerAttackDamege}");
            }
        }

    }


}
