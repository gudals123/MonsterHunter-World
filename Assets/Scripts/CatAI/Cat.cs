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

    [Header("CatController Info")]
    private CatController catController;
    private Vector3 startPosition;
    [SerializeField] private Collider catCollider;
    public int currentHP;

    [Header("Target Info")]
    private Transform target;
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;

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
            startPosition = transform.position;
            gameObject.SetActive(false);
        }
    }

    public int Heal()
    {
        heal = 20;
        return heal;
    }

    public void Respawn()
    {
        currentHp = maxHp;
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    public void PlayerTracking() // 출력
    {
        // 플레이어가 감지범위 내에 있을 때
        if (catController.catState == CatState.Detect && target.CompareTag("Player") /*catController.dir.magnitude <= detectRange && catController.dir.magnitude > interactionRange*/)
        {
            Debug.Log("Player, dir.magnitude <= detectRange");
            LookAtTarget(target);
            Move(Time.deltaTime, player.position);
            animator.Play("Tracking");
        }
        // 플레이어가 상호작용범위 내에 있을 때
        if (catController.catState == CatState.Idle /*&& catController.dir.magnitude <= interactionRange*/)
        {
            Debug.Log("Player, dir.magnitude <= interactionRange");
            LookAtTarget(target);
            animator.Play("Idle");
        }
        else
        {
            Move(Time.deltaTime, player.transform.position);
            animator.Play("Tracking");
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