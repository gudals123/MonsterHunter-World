using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnjanathAnimationEvents : MonoBehaviour
{
    public GameObject HeadColliderObj;

    private void Awake()
    {
        HeadColliderObj.SetActive(false);
    }

    public void AttackCollierOn()
    {
        HeadColliderObj.SetActive(true);
    }

    public void AttackCollierOff()
    {
        HeadColliderObj.SetActive(false);
    }
}