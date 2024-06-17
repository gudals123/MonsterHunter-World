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
    private int damage;
    private int heal;

    [Header("Cat Info")]
    private CatController catController;
    private Vector3 startPosition;
    [SerializeField] private Collider catCollider;
    public int currentHP;

    [Header("Target Info")]
    private Transform target;
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

    public override int Attack()
    {
        if (Vector3.Distance(target.position, transform.position) > catController.interactionRange)
        {
            Move(Time.deltaTime, target.position);
        }

        animator.Play("Attack");
        Debug.Log("Attack!");
        return damage;
    }

    public override void Hit(int damage)
    {
        animator.Play("Hit");
        currentHp -= damage;
        if (catController.catState == CatState.Dead)
        {
            gameObject.SetActive(false);
        }
    }

    public int Heal()
    {
        heal = 20;
        return heal;
    }

    public void Resurrection()
    {
        currentHp = maxHp;
        startPosition = player.transform.position;
        gameObject.SetActive(true);
    }

    public void PlayerTracking() // 출력
    {
        // 플레이어가 감지범위 내에 있을 때
        if (catController.catState == CatState.Detect /*&& player.CompareTag("Player")*/ /*catController.dir.magnitude <= detectRange && catController.dir.magnitude > interactionRange*/)
        {
            Debug.Log("Player, dir.magnitude <= detectRange");
            LookAtTarget(player.transform);
            Move(Time.deltaTime, player.transform.position);
            animator.Play("Tracking");
        }
        // 플레이어가 상호작용범위 내에 있을 때
        if (catController.catState == CatState.Idle /*&& catController.dir.magnitude <= interactionRange*/)
        {
            Debug.Log("Player, dir.magnitude <= interactionRange");
            LookAtTarget(player.transform);
            animator.Play("Idle");
        }
        else
        {
            Move(Time.deltaTime, player.transform.position);
            animator.Play("Tracking");
        }
    }

    public void BossTracking(Collider target)
    {
        if (playerController.playerState == PlayerController.PlayerState.Attack)
        {
            //boss = target.GetComponentInParent<Monster>();
        }

        if (player.isArmed)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < catController.detectRange
                && Vector3.Distance(target.transform.position, transform.position) > catController.interactionRange)
            {
                transform.position -= target.transform.position;
            }

            if (Vector3.Distance(target.transform.position, transform.position) < catController.interactionRange)
            {
                Attack();
                animator.Play("Attack");
            }
        }

        else
        {
            PlayerTracking();
        }
    }

    public void LookAtTarget(Transform target)
    {
        Vector3 dir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(catController.transform.position.x, 0, catController.transform.position.z);
        catController.transform.rotation = Quaternion.Lerp(catController.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
    }

    private void Update()
    {
        target = catController.target;
        LookAtTarget(target);
        PlayerTracking();
        this.currentHP = currentHp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 8f);
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}