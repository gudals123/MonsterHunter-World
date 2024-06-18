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
    [SerializeField] private Player playerObj;
    [SerializeField] public Transform boss;
    [SerializeField] public Transform target;

    [Header("Range Info")]
    //public float detectRange;
    //public float interactionRange;
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
        playerObj = GameObject.Find("Player").GetComponent<Player>();
        target = player;
        catState = CatState.Tracking;
    }

    public void Hit()
    {
        catState = CatState.Hit;
        cat.Hit(cat.damage);
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

    // public void Tracking() // 상태만 변경
    // {
    //     // 보스 공격
    //     if (dir.magnitude <= interactionRange)
    //     {
    //         if (playerController.playerState == PlayerState.Attack)
    //         {
    //             isBoss = true;
    //             CatStateAttack();
    //             return;
    //         }

    //         catState = CatState.Tracking;
    //         cat.Tracking();
    //     }

    //     // 플레이어가 감지범위 내에 있을 때
    //     else if (target.CompareTag("Player") && dir.magnitude > interactionRange)
    //     {
    //         Debug.Log("catController.PlayerTracking && dir.magnitude > interactionRange");
    //         catState = CatState.Detect;
    //         cat.Tracking();
    //     }
    //     // 플레이어가 상호작용범위 내에 있을 때
    //     else if (target.CompareTag("Player") && dir.magnitude <= interactionRange)
    //     {
    //         Debug.Log("catController.PlayerTracking && dir.magnitude <= interactionRange");
    //         catState = CatState.Idle;
    //         cat.Tracking();
    //     }
    // }

    public void Tracking()
    {
        //player
        if (isPlayer && distance > 1.5f)
        {
            catState = CatState.Tracking;
        }
        else if (isPlayer && distance <= 1.5f)
        {
            catState = CatState.Idle;
        }

        //boss
        if (isAttack && distance > 1.5f)
        {
            catState = CatState.Tracking;
        }
        else if (isAttack && distance <= 1.5f)
        {
            catState = CatState.Attack;
        }
        
        if(attackDuration > 5f)
        {
            isAttack = false;
            isPlayer = true;
            attackDuration = 0;
            catState = CatState.Tracking;
        }
        //cat.Tracking();
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
        dir = Detect(target);
        distance = Vector3.Distance(transform.position, target.position);

        if (respawnTime > 10)
        {
            cat.Respawn();
        }

        if(isAttack)
        {
            attackDuration += Time.deltaTime;
        }

        Debug.Log($"isAttack : {isAttack}");
        Debug.Log($"isPlayer : {isPlayer}");
        Debug.Log($"catState : {catState}");
        Debug.Log($"attackDuration : {attackDuration}");
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