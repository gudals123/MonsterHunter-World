using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnjanathHeadache : MonoBehaviour
{
    public Anjanath anjanath;
    private void Awake()
    {
        //anjanath = GetComponentInParent<Anjanath>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            anjanath.weakness = true;
        }
    }
}
