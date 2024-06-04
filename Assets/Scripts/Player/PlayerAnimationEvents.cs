using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject _handWeapon;
    [SerializeField] private GameObject _BackWeapon;

    private void Awake()
    {
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
        GetComponent<Animator>().speed = 0.0001f;
    }


}
