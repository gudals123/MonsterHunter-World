using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_CatHeal : Slot
{
    private Cat cat;
    private Player player;
    public Skill_CatHeal(Cat _cat, Player _player)
    {
        cat = _cat;
        player = _player;
        UIManager.Instance.SetIcon(this);
    }

    public override void Activate()
    {
        player.HealEffectPlay();
        Debug.Log($"CatHeal");
        cat.Heal();
    }



}
