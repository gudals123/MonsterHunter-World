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

    [Header("Cat Info")]
    private CatController catController;
    private Vector3 startPosition;
    [SerializeField] private Collider catCollider;
    //private Vector3 detectRange;

    [Header("Target Info")]
    private Transform target;
    [SerializeField] private Player player;
    [SerializeField] private Monster boss;

    [SerializeField] private PlayerController playerController;
    //[Header("Range Info")]
    //private float detectRange;
    //private float interactionRange;


    private void Awake()
    {
        currentHp = maxHp;

        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        catController = GetComponent<CatController>();
        animator = GetComponentInChildren<Animator>();
        //player = GameObject.Find("Player").GetComponent<Player>();
        //playerController = GetComponent<PlayerController>();

        target = player.transform;
        PlayerTracking();
        //target = catController.target;
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
        damage = Random.Range(1, 6);
        return damage;
    }

    public void SkillAttack(Transform target)
    {
        Move(Time.deltaTime, target.position);
        Attack();
    }

    public override void Hit(int damage)
    {
        animator.Play("Hit");
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
        else if (skill == "SkillAttack")
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
        startPosition = player.transform.position;
        gameObject.SetActive(true);
    }

    public void PlayerTracking() // 출력
    {
        //// 보스가 감지범위 내에 있을 때
        //if (catController.catState == CatState.Detect && target.CompareTag("Boss") /*catController.dir.magnitude <= detectRange && catController.dir.magnitude > interactionRange*/)
        //{
        //    Debug.Log("Boss, dir.magnitude <= detectRange");
        //    LookAtTarget(target);
        //    Move(Time.deltaTime, boss.position);
        //    animator.Play("PlayerTracking");
        //}
        //// 보스가 상호작용범위 내에 있을 때
        //if (catController.catState == CatState.Attack && catController.dir.magnitude <= catController.interactionRange)
        //{
        //    Debug.Log("Boss, dir.magnitude <= interactionRange");
        //    LookAtTarget(target);
        //    Attack();
        //}

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

        //switch (catController.catState)
        //{
        //    case CatState.Detect:
        //        LookAtTarget(player.transform);
        //        Move(Time.deltaTime, player.transform.position);
        //        animator.Play("Tracking");
        //        break;
        //    case CatState.Idle:
        //        LookAtTarget(player.transform);
        //        animator.Play("Idle");
        //        break;
        //    default:
        //        Move(Time.deltaTime, player.transform.position);
        //        animator.Play("Tracking");
        //        break;
        //}
        // else 플레이어 트래킹
        //if (/*!(catController.catState == CatState.Detect) ||*/ target != boss /*&& catController.dir.magnitude > interactionRange*/)
        //{
        //    Debug.Log("else Player");
        //    target = player;
        //    LookAtTarget(player);
        //    Move(Time.deltaTime, player.position);
        //}

        //if (target == null)
        //{
        //    Debug.Log("else Player");
        //    target = player;
        //    LookAtTarget(player);
        //    Move(Time.deltaTime, player.position);
        //}
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (target == null)
    //    {
    //        target = player;
    //    }

    //    if (target != null)
    //    {
    //        target = other.transform;
    //    }
    //}

    private void Update()
    {
        target = catController.target;
        LookAtTarget(target);
        PlayerTracking();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, catController.detectRange);
        Gizmos.DrawWireSphere(transform.position, catController.interactionRange);
    }
}