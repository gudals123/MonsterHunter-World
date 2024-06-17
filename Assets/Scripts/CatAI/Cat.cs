using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static CatController;

public class Cat : Entity
{
    [Header("Cat Info")]
    private float respawnTime;
    private int heal;
    public int damage;

    [Header("CatController Info")]
    private CatController catController;
    private Vector3 startPosition;
    [SerializeField] private Collider catCollider;
    [HideInInspector] public int currentHP;

    [Header("Target Info")]
    public Transform target;
    [SerializeField] private Player player;
    [SerializeField] private Monster boss;

    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        currentHp = maxHp;

        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        catController = GetComponent<CatController>();
        animator = GetComponentInChildren<Animator>();

        target = player.transform;
    }

    public override void Move(float moveSpeed, Vector3 targetPos)
    {
        animator.Play("Tracking");
        catController.transform.position -= catController.Detect(targetPos) * moveSpeed;
    }

    public void Attack(Transform target)
    {
        if (Vector3.Distance(target.position, transform.position) > catController.interactionRange)
        {
            Move(Time.deltaTime, target.position);
        }

        animator.Play("Attack");
    }

    public override void Hit(int damage)
    {
        animator.Play("Hit");
        currentHp -= damage;
        if (catController.catState == CatState.Dead)
        {
            startPosition = transform.position;
            gameObject.SetActive(false);
        }
    }

    public void Heal()
    {
        if (catController.catState == CatState.Skill /*|| player.currentHp <= 30*/)
        {
            heal = 10;
            player.Heal(heal);
        }
    }

    public void Respawn()
    {
        currentHp = maxHp;
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    // public void Tracking() // 출력
    // {
    //     if(catController.isAttack && catController.dir.magnitude <= catController.interactionRange)
    //     {
    //         // boss = target.GetComponentInParent<Monster>(); // player에서 target 설정?

    //         Attack(target.transform);
    //     }
        
    //     if(catController.isAttack && catController.dir.magnitude > catController.interactionRange)
    //     {
    //         Move(Time.deltaTime, boss.transform.position);
    //     }

    //     // 플레이어가 감지범위 내에 있을 때
    //     else if (catController.catState == CatState.Detect)
    //     {
    //         Debug.Log("Player, dir.magnitude <= detectRange");
    //         LookAtTarget(player.transform);
    //         Move(Time.deltaTime, player.transform.position);
    //     }

    //     // 플레이어가 상호작용범위 내에 있을 때
    //     else if (catController.catState == CatState.Idle)
    //     {
    //         Debug.Log("Player, dir.magnitude <= interactionRange");
    //         LookAtTarget(player.transform);
    //         isAttack = false;
    //         animator.Play("Idle");
    //     }
    // }

    public void Tracking()
    {
        if(catController.isAttack && catController.catState == CatState.Tracking)
        {
            Move(Time.deltaTime, boss.transform.position);
        }

        else if(catController.isAttack && catController.catState == CatState.Idle)
        {
            Attack(boss.transform);
        }

        else if(catController.isPlayer && catController.catState == CatState.Tracking)
        {
            Move(Time.deltaTime, player.transform.position);
        }

        else if(catController.isPlayer && catController.catState == CatState.Attack)
        {
            animator.Play("Idle");
        }
    }

    public void LookAtTarget(Transform target)
    {
        Vector3 dir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(catController.transform.position.x, 0, catController.transform.position.z);
        catController.transform.rotation = Quaternion.Lerp(catController.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
    }

    public void SetTarget(Collider other)
    {
        target = other.transform;
    }

    private void Update()
    {
        LookAtTarget(target);
        this.currentHP = currentHp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 8f);
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}