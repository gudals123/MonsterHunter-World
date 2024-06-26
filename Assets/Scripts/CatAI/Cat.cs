using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static CatController;

public class Cat : Entity
{
    [Header("Cat Info")]
    private int heal;
    public int damage;

    [Header("CatController Info")]
    private CatController catController;
    private Vector3 startPosition;
    [SerializeField] private Collider catCollider;
    [HideInInspector] public int currentHP;

    [Header("Target Info")]
    [SerializeField] private Player player;


    private void Awake()
    {
        maxHp = 80;
        currentHp = maxHp;

        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        catController = GetComponent<CatController>();
        animator = GetComponentInChildren<Animator>();

        catController.target = player.transform;
    }

    public override void Move(float moveSpeed, Transform targetPos)
    {
        animator.Play("Tracking");
        LookAtTarget(targetPos);
        catController.transform.position -= catController.Detect(targetPos) * moveSpeed;
    }

    public void Attack(Transform target)
    {
        LookAtTarget(target);
        if (catController.distance > 4f)
        {
            Move(Time.deltaTime / 3, target);
        }
        animator.Play("Attack");
    }

    public override void Hit(int damage)
    {
        currentHp -= damage;
        if (catController.catState == CatState.Dead)
        {
            startPosition = transform.position;
            gameObject.SetActive(false);
        }
    }

    public void Heal()
    {
        heal = 10;
        player.Heal(heal);
    }

    public void Respawn()
    {
        currentHp = maxHp;
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    public void Tracking()
    {
        if (catController.isAttack && catController.catState == CatState.Tracking)
        {
            Move(Time.deltaTime / 3, catController.target.transform);
        }

        else if (catController.isAttack && catController.catState == CatState.Attack)
        {
            Attack(catController.target.transform);
        }

        else if (catController.isPlayer && catController.catState == CatState.Tracking)
        {
            Move(Time.deltaTime / 3, catController.target.transform);
        }

        else if (catController.isPlayer && catController.catState == CatState.Idle)
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
        catController.target = other.transform;
        catController.isAttack = true;
    }

    private void Update()
    {
        this.currentHP = currentHp;
    }
}