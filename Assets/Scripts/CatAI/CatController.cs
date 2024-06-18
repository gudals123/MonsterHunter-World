using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Unity.VisualScripting;
using static CatController;

public class CatController : AIController
{
    public enum CatState
    {
        Idle,
        Move,
        Hit,
        Dead,
        Detect,
        Tracking,
        Attack,
        Skill
    }

    private Cat cat;
    public CatState catState;

    [SerializeField] public Transform player;
    [SerializeField] public Transform target;

    [Header("Range Info")]
    public Vector3 dir;
    public float distance;

    public bool isPlayer;
    public bool isBoss;
    public bool isAttack;

    private float respawnTime;
    public float attackDuration;

    private void Start()
    {
        cat = GetComponent<Cat>();
        target = player;
        catState = CatState.Tracking;
    }

    public void Hit()
    {
        catState = CatState.Hit;
        if (cat.currentHP <= 0)
        {
            catState = CatState.Dead;
            respawnTime += Time.deltaTime;
        }
    }

    public Vector3 Detect(Transform targetPos)
    {
        Vector3 direction = (transform.position - targetPos.position);

        return direction;
    }

    public void Distance()
    {
        distance = Vector3.Distance(transform.position, target.position);
    }

    public void Tracking()
    {
        //player
        if (isPlayer && distance > 4f)
        {
            catState = CatState.Tracking;
        }
        else if (isPlayer && distance <= 4f)
        {
            catState = CatState.Idle;
        }

        //boss
        if (distance < 10f && isAttack && distance > 4f)
        {
            catState = CatState.Tracking;
        }
        else if (isAttack && distance <= 4f)
        {
            catState = CatState.Attack;
        }

        if (attackDuration > 10f)
        {
            isAttack = false;
            isPlayer = true;
            target = player;
            attackDuration = 0;
            catState = CatState.Tracking;
        }
        cat.Tracking();
    }

    public void TargetCheck()
    {
        // bool 값 변경
        if (isAttack)
        {
            isPlayer = false;
        }

        else if (!isAttack)
        {
            isPlayer = true;
        }
    }

    public void Heal()
    {
        catState = CatState.Skill;
    }

    private void Update()
    {
        TargetCheck();
        Tracking();
        distance = Vector3.Distance(transform.position, target.position);

        if (respawnTime > 10)
        {
            cat.Respawn();
        }

        if (isAttack)
        {
            attackDuration += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Plane")
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            isPlayer = true;
        }

        if (other.CompareTag("BossAttack"))
        {
            isPlayer = false;
            catState = CatState.Hit;
            cat.damage = other.GetComponent<BossAttackMethod>().attackDamage;
        }
    }
}