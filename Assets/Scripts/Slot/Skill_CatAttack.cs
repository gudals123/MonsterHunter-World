using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_CatAttack : Slot
{
    Cat cat;
    public Skill_CatAttack(Cat _cat)
    {
        cat = _cat;
    }

    public override void Activate()
    {
        Debug.Log($"CatAttack");
        CatAttack();
    }

    private void CatAttack()
    {
        //cat.SkillAttack(Transform monster);
    }
}


