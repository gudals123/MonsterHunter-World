using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_CatHeal : Slot
{
    private Cat cat;
    public Skill_CatHeal(Cat _cat)
    {
        cat = _cat;
    }

    public override void Activate()
    {
        Debug.Log($"CatHeal");
        CatHeal();
    }

    private void CatHeal()
    {
        //cat.Heal();
    }

}
