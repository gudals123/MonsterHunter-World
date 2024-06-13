using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    }

    public override void Activate()
    {
        if(count > 0)
        {
            Debug.Log($"포션 사용 count: {count}");
            Heal();
            count--;
        }
    }

    private void Heal()
    {
        player.Heal(healingAmount);
    }

}
