using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class Item_Potion : Slot
{
    private Player player;
    private int healingAmount;
    public int count;
    public Item_Potion(Player _player , int _healingAmount , int _count)
    {
        player = _player;
        healingAmount = _healingAmount;
        count = _count;
        UIManager.Instance.SetIcon(this);
    }


    public override void Activate()
    {
        if(count > 0)
        {
            Heal();
            count--;
            Debug.Log($"포션 사용 count: {count}");
        }
    }

    private void Heal()
    {
        player.Heal(healingAmount);
    }

}
