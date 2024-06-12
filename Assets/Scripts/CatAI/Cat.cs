using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static CatController;

public class Cat : Entity
{
    [Header("Cat Info")]
    private string skill;
    private float respawnTime;
    private int damage;
    private int heal;
    private CatController catController;
    [SerializeField] private Collider catCollider;
    //private Vector3 detectRange;

    [Header("Target Info")]
    //private Entity target;
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;

    [Header("Range Info")]
    private float detectRange;
    private float interactionRange;


    private void Awake()
    {
        currentHp = maxHp;
        //startPosition = GetComponent<Rigidbody>().position;

        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        catController = GetComponent<CatController>();

        detectRange = 8f;
        interactionRange = 3f;
    }

    public override void Move(float moveSpeed, Vector3 targetPos)
    {
        catController.transform.position += catController.Detect(targetPos) / moveSpeed;
    }

    public override int Attack()
    {
        animator.Play("Attack");
        damage = Random.Range(1, 6);
        return damage;
    }

    public override void Hit(int damage)
    {
        animator.Play("GetHit");
        currentHp -= damage;
        catController.catState = CatState.Hit;
        if (currentHp <= 0)
        {
            catController.catState = CatState.Dead;
            gameObject.SetActive(false);
        }
    }

    public void Skill()
    {
        if (skill == "Heal")
        {
            Heal();
        }
        else if (skill == "Attack")
        {

            Attack();
        }
        else if(skill == "SkillAttack")
        {
            // 스킬 추가
            Attack();
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
        //startPosition = transform.position;
        gameObject.SetActive(true);
    }

    public void Tracking(Transform target)
    {
        Vector3 dir = catController.Detect(target.position);

        // 보스가 감지범위 내에 있을 때
        if (target.CompareTag("Boss") && dir.magnitude <= detectRange)
        {
            Debug.Log("Boss, dir.magnitude <= detectRange");
            animator.Play("Move");
            transform.position += dir;
        }
        // 보스가 상호작용범위 내에 있을 때
        if (target.CompareTag("Boss") && dir.magnitude <= interactionRange)
        {
            Debug.Log("Boss, dir.magnitude <= interactionRange");
            animator.Play("Attack");
            Attack();
        }
        // 플레이어가 상호작용범위 내에 있을 때
        if (target.CompareTag("Player") && dir.magnitude <= interactionRange)
        {
            Debug.Log("Player, dir.magnitude <= interactionRange");
            animator.Play("Idle");
        }
        // else 플레이어 트래킹
        else
        {
            Debug.Log("else Player");
            target = player;
            animator.Play("Move");
        }
    }

    public void LookAtTarget(Transform target)
    {
        Vector3 dir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(catController.transform.position.x, 0, catController.transform.position.z);
        catController.transform.rotation = Quaternion.Lerp(catController.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
    }
}