using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWeapon : Weapon
{
    [SerializeField] private GameObject catObject;
    private CatController catController;
    private Cat cat;


    protected override void Awake()
    {
        base.Awake();
        catController = catObject.GetComponent<CatController>();
        cat = catObject.GetComponent<Cat>();
        attackDamage = 3;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss") || other.CompareTag("Monster"))
        {
            catController.catState = CatController.CatState.Attack;
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos);
            cat.Attack(other.transform);
        }
    }
}
