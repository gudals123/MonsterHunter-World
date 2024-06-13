using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnjanathController : AIController
{
    public AnjanathBT anjanathBT;
    private Anjanath anjanath;

    protected bool checkTarget;
    public int getOtherAttackDamage;

    private void Awake()
    {
        anjanathBT = GetComponent<AnjanathBT>();
        anjanath = GetComponent<Anjanath>();
    }

    private void Update()
    {
        anjanath.IsPlayerInRange();
    }

    public void Hit(int damage)
    {
        anjanathBT.anjanathState = State.SetDamage;    // @@@@@@@
    }

}
