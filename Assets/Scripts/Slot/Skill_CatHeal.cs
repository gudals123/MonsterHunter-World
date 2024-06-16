using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_CatHeal : Slot
{
    private Cat cat;
    public Skill_CatHeal(Cat _cat)
    {
        cat = _cat;
        UIManager.Instance.SetIcon(this);
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
