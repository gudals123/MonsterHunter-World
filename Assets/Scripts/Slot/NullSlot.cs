using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NullSlot : Slot
{

    public NullSlot()
    {
        UIManager.Instance.SetIcon(this);
    }

    public override void Activate()
    {
        Debug.Log($"NUll");
    }
}



