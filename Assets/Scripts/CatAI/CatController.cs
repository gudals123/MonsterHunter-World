using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Unity.VisualScripting;

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
    [SerializeField] public Transform boss;
    [SerializeField] public Transform target;

    public bool isPlayer = true;
    public bool isBoss;

    [Header("Range Info")]
    public float detectRange;
    public float interactionRange;
    public Vector3 dir;

    private float respawnTime;

    private void Start()
    {
        cat = GetComponent<Cat>();
        target = player;
        isPlayer = true;
        catState = CatState.Tracking;
        detectRange = 8f;
        interactionRange = 1.5f;
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

    public Vector3 Detect(Vector3 targetPos)
    {
        Vector3 direction = (transform.position - targetPos);

        return direction;
    }

    public void PlayerTracking() // 상태만 변경
    {
        // 플레이어가 감지범위 내에 있을 때
        if (target.CompareTag("Player") && dir.magnitude > interactionRange)
        {
            Debug.Log("catController.PlayerTracking && dir.magnitude > interactionRange");
            isBoss = false;
            isPlayer = true;
            catState = CatState.Detect;
            cat.PlayerTracking();
        }
        // 플레이어가 상호작용범위 내에 있을 때
        if (target.CompareTag("Player") && dir.magnitude <= interactionRange)
        {
            Debug.Log("catController.PlayerTracking && dir.magnitude <= interactionRange");
            isBoss = false;
            isPlayer = true;
            catState = CatState.Idle;
            cat.PlayerTracking();
        }
        else
        {
            target = player;
            catState = CatState.Detect;
        }
    }

    public void BossTracking()
    {
        if (Vector3.Distance(boss.transform.position, transform.position) < detectRange
            && Vector3.Distance(boss.transform.position, transform.position) > interactionRange)
        {
            isPlayer = false;
            isBoss = true;
            catState = CatState.Detect;
            cat.BossTracking();
        }

        if (Vector3.Distance(boss.transform.position, transform.position) < interactionRange)
        {
            isPlayer = false;
            isBoss = true;
            catState = CatState.Attack;
            cat.BossTracking();
        }
    }

    public void Heal()
    {
        catState = CatState.Skill;
    }

    private void Update()
    {
        dir = Detect(target.position);

        if (respawnTime > 10)
        {
            cat.Respawn();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Plane")
        {
            return;
        }

        if (other.CompareTag("BossAttack"))
        {
            catState = CatState.Hit;
            cat.damage = other.GetComponent<BossAttackMethod>().attackDamage;
        }

        //else
        //{
        //    target = other.transform;
        //    Debug.Log($"trigger : {target.tag}");
        //}
    }
}